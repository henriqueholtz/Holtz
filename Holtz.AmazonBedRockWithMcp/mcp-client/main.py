import asyncio
import sys
from typing import Optional, List, Dict, Any
from contextlib import AsyncExitStack
from dataclasses import dataclass
from message import Message
from client import MCPClient

async def main():
    """
    Main entry point that sets up the client and starts the chat loop.
    Requires a server script path as a command line argument.
    """
    if len(sys.argv) < 2:
        print("Usage: python main.py <path_to_server_script>")
        sys.exit(1)

    client = MCPClient()
    try:
        await client.connect_to_server(sys.argv[1])
        await client.chat_loop()
    finally:
        await client.cleanup()

if __name__ == "__main__":
    asyncio.run(main())