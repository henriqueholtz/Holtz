import asyncio
import sys
from typing import Optional, List, Dict, Any
from contextlib import AsyncExitStack
from dataclasses import dataclass
from message import Message

# to interact with MCP
from mcp import ClientSession, StdioServerParameters
from mcp.client.stdio import stdio_client

# to interact with Amazon Bedrock
import boto3
 
class MCPClient:
    """
    A client that connects to an MCP server and uses Amazon Bedrock for AI interactions.
    Handles the communication between the user, MCP tools, and the Bedrock AI model.
    """
    MODEL_ID = "anthropic.claude-3-sonnet-20240229-v1:0"
    
    def __init__(self):
        """
        Initialize the MCP client with empty session and create a Bedrock client.
        """
        self.session: Optional[ClientSession] = None
        self.exit_stack = AsyncExitStack()
        self.bedrock = boto3.client(service_name='bedrock-runtime', region_name='us-east-1')

    async def connect_to_server(self, server_script_path: str):
        """
        Connect to the MCP server using the specified script.
        Args:
            server_script_path: Path to the server script (.py or .js file)
        Raises:
            ValueError: If the server script is not a .py or .js file
        """
        if not server_script_path.endswith(('.py', '.js')):
            raise ValueError("Server script must be a .py or .js file")

        command = "python" if server_script_path.endswith('.py') else "node"
        server_params = StdioServerParameters(command=command, args=[server_script_path], env=None)

        stdio_transport = await self.exit_stack.enter_async_context(stdio_client(server_params))
        self.stdio, self.write = stdio_transport
        self.session = await self.exit_stack.enter_async_context(ClientSession(self.stdio, self.write))
        await self.session.initialize()

        response = await self.session.list_tools()
        print("\nConnected to server with tools:", [tool.name for tool in response.tools])

    async def cleanup(self):
        """
        Clean up resources by closing the async exit stack.
        """
        await self.exit_stack.aclose()

    def _make_bedrock_request(self, messages: List[Dict], tools: List[Dict]) -> Dict:
        """
        Make a request to Amazon Bedrock with the given messages and tools.
        Args:
            messages: List of conversation messages
            tools: List of available tools
        Returns:
            Response from Bedrock
        """
        return self.bedrock.converse(
            modelId=self.MODEL_ID,
            messages=messages,
            inferenceConfig={"maxTokens": 1000, "temperature": 0},
            toolConfig={"tools": tools}
        )

    async def process_query(self, query: str) -> str:
        """
        Process a user query through the MCP server and Bedrock.
        Args:
            query: The user's input query
        Returns:
            The final response text
        """
        # (1)
        messages = [Message.user(query).__dict__]
        # (2)
        response = await self.session.list_tools()

        # (3)
        available_tools = [{
            "name": tool.name,
            "description": tool.description,
            "input_schema": tool.inputSchema
        } for tool in response.tools]

        bedrock_tools = Message.to_bedrock_format(available_tools)

        # (4)
        response = self._make_bedrock_request(messages, bedrock_tools)

        # (6)
        return await self._process_response( # (5)
          response, messages, bedrock_tools
        )
    
    async def _process_response(self, response: Dict, messages: List[Dict], bedrock_tools: List[Dict]) -> str:
        """
        Process the response from Bedrock, handling tool calls and conversation flow.
        Args:
            response: The response from Bedrock
            messages: The conversation history
            bedrock_tools: List of available tools
        Returns:
            The final response text after processing all turns
        """
        # (1)
        final_text = []
        MAX_TURNS=10
        turn_count = 0

        while True:
            # (2)
            if response['stopReason'] == 'tool_use':
                final_text.append("received toolUse request")
                for item in response['output']['message']['content']:
                    if 'text' in item:
                        final_text.append(f"[Thinking: {item['text']}]")
                        messages.append(Message.assistant(item['text']).__dict__)
                    elif 'toolUse' in item:
                        # (3)
                        tool_info = item['toolUse']
                        result = await self._handle_tool_call(tool_info, messages)
                        final_text.extend(result)
                        
                        response = self._make_bedrock_request(messages, bedrock_tools)
            # (4)
            elif response['stopReason'] == 'max_tokens':
                final_text.append("[Max tokens reached, ending conversation.]")
                break
            elif response['stopReason'] == 'stop_sequence':
                final_text.append("[Stop sequence reached, ending conversation.]")
                break
            elif response['stopReason'] == 'content_filtered':
                final_text.append("[Content filtered, ending conversation.]")
                break
            elif response['stopReason'] == 'end_turn':
                final_text.append(response['output']['message']['content'][0]['text'])
                break

            turn_count += 1

            if turn_count >= MAX_TURNS:
                final_text.append("\n[Max turns reached, ending conversation.]")
                break
        # (5)
        return "\n\n".join(final_text)

    async def _handle_tool_call(self, tool_info: Dict, messages: List[Dict]) -> List[str]:
        """
        Handle a tool call request from Bedrock.
        Args:
            tool_info: Information about the tool call
            messages: The conversation history
        Returns:
            List of strings describing the tool call and its result
        """
        # (1)
        tool_name = tool_info['name']
        tool_args = tool_info['input']
        tool_use_id = tool_info['toolUseId']

        # (2)
        result = await self.session.call_tool(tool_name, tool_args)

        # (3)
        messages.append(Message.tool_request(tool_use_id, tool_name, tool_args).__dict__)
        messages.append(Message.tool_result(tool_use_id, result.content).__dict__)

        # (4)
        return [f"[Calling tool {tool_name} with args {tool_args}]"]

    async def chat_loop(self):
        """
        Main chat loop that handles user input and displays responses.
        Continues until the user types 'quit'.
        """
        print("\nMCP Client Started!\nType your queries or 'quit' to exit.")
        while True:
            try:
                query = input("\nType your query or 'quit' to leave: ").strip()
                if query.lower() == 'quit':
                    break
                response = await self.process_query(query)
                print("\n" + response)
            except Exception as e:
                print(f"\nError: {str(e)}")

