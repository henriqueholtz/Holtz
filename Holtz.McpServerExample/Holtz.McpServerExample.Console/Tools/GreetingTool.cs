using System;
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace Holtz.McpServerExample.Console.Tools;

[McpServerToolType]
public static class GreetingTool
{
    [McpServerTool, Description("Get a greeting message")]
    public static string Greet(string who) =>
        JsonSerializer.Serialize(new { message = $"Hello, {who}!" });
}