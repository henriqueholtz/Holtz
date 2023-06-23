using Holtz.RabbitMQ.SimpleConsole.Core;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Starting RabbitMQ Sender...");
Console.WriteLine();

var factory = new ConnectionFactory { HostName = HoltzRabbitMQ.HostName, UserName = HoltzRabbitMQ.User, Password = HoltzRabbitMQ.Password };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: HoltzRabbitMQ.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

bool exit = false;
while (!exit)
{
    string? message = string.Empty;
    while (string.IsNullOrEmpty(message))
    {
        Console.Write("Type your message: ");
        message = Console.ReadLine();
    }

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: HoltzRabbitMQ.RoutingKey,
                         basicProperties: null,
                         body: body);

    Console.WriteLine($"Message Sent: {message}");
    Console.WriteLine();

    Console.Write("Type 'exit' to leave or press anything else to send another message: ");
    string? exitAsString = Console.ReadLine();
    exit = !string.IsNullOrWhiteSpace(exitAsString) && exitAsString.Equals("exit", StringComparison.OrdinalIgnoreCase);
    Console.WriteLine();
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();