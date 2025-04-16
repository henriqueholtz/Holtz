using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Holtz.McpServerExample.Console.Tools;

[McpServerToolType]
public static class CalculatorTool
{

    [McpServerTool, Description("Adds two numbers")]
    public static int Add(int a, int b) => a + b;
}