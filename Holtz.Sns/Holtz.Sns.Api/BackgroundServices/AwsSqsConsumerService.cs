using System.Reflection;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Holtz.Sns.Application.Commands;
using Holtz.Sns.Shared;
using Holtz.Sns.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Holtz.Sns.Api.BackgroundServices;

public class AwsSqsConsumerService : BackgroundService
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly IMediator _mediator;
    private readonly ILogger<AwsSqsConsumerService> _logger;

    public AwsSqsConsumerService(IAmazonSQS amazonSQS, IOptions<QueueSettings> queueSettings, IMediator mediator, ILogger<AwsSqsConsumerService> logger)
    {
        _amazonSQS = amazonSQS;
        _queueSettings = queueSettings;
        _mediator = mediator;
        _logger = logger;
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
                string typeName = $"Holtz.Sns.Application.Commands.{messageTypeAsString}Command";
                Assembly assembly = typeof(CustomerCreatedCommand).Assembly;
                var type = assembly.GetType(typeName, false, true);

                if (type is null)
                {
                    _logger.LogWarning($"[AwsSqsConsumerService] Unknown type: {messageTypeAsString}. TypeName: {typeName}.");
                    continue;
                }

                var typedMessage = (ISnsMessageMarker)JsonSerializer.Deserialize(message.Body, type)!;

                try
                {
                    await _mediator.Send(typedMessage, stoppingToken);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"[AwsSqsConsumerService] Error caught during processing: {ex.Message}", ex);
                    continue;
                }

                await _amazonSQS.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                Console.WriteLine();
            }
            await Task.Delay(2000, stoppingToken);
        }
    }
}
