import asyncio
import sys
from typing import Optional, List, Dict, Any
from contextlib import AsyncExitStack
from dataclasses import dataclass

# to interact with MCP
from mcp import ClientSession, StdioServerParameters
from mcp.client.stdio import stdio_client

# to interact with Amazon Bedrock
import boto3
 
@dataclass
class Message:
    """
    A class representing a message in the conversation, with a role and content.
    Used for formatting messages in a way that's compatible with Amazon Bedrock.
    """
    role: str
    content: List[Dict[str, Any]]

    @classmethod
    def user(cls, text: str) -> 'Message':
        """
        Creates a message from the user with the given text.
        Args:
            text: The user's message text
        Returns:
            A Message object with role 'user' and the text in content
        """
        return cls(role="user", content=[{"text": text}])

    @classmethod
    def assistant(cls, text: str) -> 'Message':
        """
        Creates a message from the assistant with the given text.
        Args:
            text: The assistant's message text
        Returns:
            A Message object with role 'assistant' and the text in content
        """
        return cls(role="assistant", content=[{"text": text}])

    @classmethod
    def tool_result(cls, tool_use_id: str, content: dict) -> 'Message':
        """
        Creates a message containing the result of a tool call.
        Args:
            tool_use_id: The ID of the tool call this result corresponds to
            content: The result content from the tool call
        Returns:
            A Message object formatted as a tool result
        """
        return cls(
            role="user",
            content=[{
                "toolResult": {
                    "toolUseId": tool_use_id,
                    "content": [{"json": {"text": content[0].text}}]
                }
            }]
        )

    @classmethod
    def tool_request(cls, tool_use_id: str, name: str, input_data: dict) -> 'Message':
        """
        Creates a message representing a tool call request.
        Args:
            tool_use_id: Unique ID for this tool call
            name: Name of the tool to call
            input_data: Arguments to pass to the tool
        Returns:
            A Message object formatted as a tool request
        """
        return cls(
            role="assistant",
            content=[{
                "toolUse": {
                    "toolUseId": tool_use_id,
                    "name": name,
                    "input": input_data
                }
            }]
        )

    @staticmethod
    def to_bedrock_format(tools_list: List[Dict]) -> List[Dict]:
        """
        Converts a list of tools into the format expected by Amazon Bedrock.
        Args:
            tools_list: List of tool definitions with name, description, and schema
        Returns:
            List of tools formatted for Bedrock's API
        """
        return [{
            "toolSpec": {
                "name": tool["name"],
                "description": tool["description"],
                "inputSchema": {
                    "json": {
                        "type": "object",
                        "properties": tool["input_schema"]["properties"],
                        "required": tool["input_schema"]["required"]
                    }
                }
            }
        } for tool in tools_list]
   