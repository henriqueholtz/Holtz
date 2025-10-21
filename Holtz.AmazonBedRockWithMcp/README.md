# Holtz.AmazonBedRockWithMcp

https://community.aws/content/2uFvyCPQt7KcMxD9ldsJyjZM1Wp/model-context-protocol-mcp-and-amazon-bedrock?lang=en#prerequisites

### Prerequisites

1. Docker (it's all built in by using DevContainers)
2. AWS Account with the permission `AmazonBedrockFullAccess`

### Run and test it ou

1. Open the project folder with DevContainers
2. Run the following commands:
    - `aws configure` (set up your AWS Credentials here). Note: The credentials from `.env` is not being recognized sometimes;
    - `source .venv/bin/activate` for activating the env 
    - `cd mcp-client/ && uv run main.py ../weather/weather.py` to run both the MCP Client and MCP Server

Then you'll be able to ask something (prompt/query).