using System;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Holtz.Sqs.Shared;
using Holtz.Sqs.Shared.Messages;
using Microsoft.Extensions.Options;

namespace Holtz.Sqs.Api.BackgroundServices;

public class AwsSqsConsumerService : BackgroundService
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly IOptions<QueueSettings> _queueSettings;

    public AwsSqsConsumerService(IAmazonSQS amazonSQS, IOptions<QueueSettings> queueSettings)
    {
        _amazonSQS = amazonSQS;
        _queueSettings = queueSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[AwsSqsConsumerService] Initializing the background service...");
        var queueUrlResponse = await _amazonSQS.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);

        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = new List<string> { "All" },
            MessageAttributeNames = new List<string> { "All" },
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _amazonSQS.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
            foreach (var message in response.Messages)
            {
                Console.WriteLine($"[AwsSqsConsumerService] New message consumed (id: {message.MessageId})...");
                string? messageTypeAsString = message.MessageAttributes["MessageType"]?.StringValue;
                switch (messageTypeAsString)
                {
                    case nameof(CustomerCreated):
                        var customerCreated = JsonSerializer.Deserialize<CustomerCreated>(message.Body);
                        Console.WriteLine($"[AwsSqsConsumerService] Customer id {customerCreated?.Id} created.");
                        break;
                    case nameof(CustomerUpdated):
                        var customerUpdated = JsonSerializer.Deserialize<CustomerUpdated>(message.Body);
                        Console.WriteLine($"[AwsSqsConsumerService] Customer id {customerUpdated?.Id} updated.");
                        break;
                    case nameof(CustomerDeleted):
                        var customerDeleted = JsonSerializer.Deserialize<CustomerDeleted>(message.Body);
                        Console.WriteLine($"[AwsSqsConsumerService] Customer id {customerDeleted?.Id} deleted.");
                        break;
                    default:
                        Console.WriteLine($"[AwsSqsConsumerService] Unknwon message type: {messageTypeAsString}");
                        break;
                }

                await _amazonSQS.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                Console.WriteLine();
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
}
