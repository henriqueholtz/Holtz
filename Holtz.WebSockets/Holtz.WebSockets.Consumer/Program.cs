// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Text;

Console.WriteLine("Initializing a consumer...");

using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri("ws://localhost:5050/ws"), CancellationToken.None);

byte[] buf = new byte[1056];

var result = await ws.ReceiveAsync(buf, CancellationToken.None);

if (result.MessageType == WebSocketMessageType.Close)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
    Console.WriteLine(result.CloseStatusDescription);
}
else
{
    Console.WriteLine(Encoding.ASCII.GetString(buf, 0, result.Count));
}
Console.ReadLine();