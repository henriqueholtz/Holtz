# Holtz.McpServerExample

https://github.com/modelcontextprotocol/csharp-sdk

### Requirements

- NodeJs
- .NET SDK

### Steps to run

- `dotnet build`
- `docker run -p 5173:5173 -it --rm mcp/inspector dotnet run --project ./Holtz.McpServerExample.Console/Holtz.McpServerExample.Console.csproj` or `npx @modelcontextprotocol/inspector dotnet run --project ./Holtz.McpServerExample.Console/Holtz.McpServerExample.Console.csproj`
- Acess the url in the browser (usually [http://127.0.0.1:6274/](http://127.0.0.1:6274/))
  - Click on "Connect"
  - In the Tools tab, click on "List tools" and select the tool you want to use
