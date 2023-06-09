﻿using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Holtz.RabbitMQ.SimpleConsole.Core;

Console.WriteLine("Starting RabbitMQ Consumer...");
Console.WriteLine();

var factory = new ConnectionFactory { HostName = HoltzRabbitMQ.HostName, UserName = HoltzRabbitMQ.User, Password = HoltzRabbitMQ.Password };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: HoltzRabbitMQ.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message Received: {message}");
};
channel.BasicConsume(queue: HoltzRabbitMQ.QueueName,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();