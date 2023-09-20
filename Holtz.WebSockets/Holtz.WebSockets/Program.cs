using System.Net.WebSockets;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var rand = new Random();

        while (true)
        {
            var now = DateTime.Now;
            byte[] data = Encoding.ASCII.GetBytes($"{now} connected!");
            await webSocket.SendAsync(data, WebSocketMessageType.Text,
                true, CancellationToken.None);
            await Task.Delay(1000);

            long r = rand.NextInt64(0, 10);

            if (r == 7)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "random closing", CancellationToken.None);

                return;
            }
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.Run("http://localhost:5050");
