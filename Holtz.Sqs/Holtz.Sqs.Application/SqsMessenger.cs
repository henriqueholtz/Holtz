using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Holtz.Sqs.Application.Interfaces;
using Holtz.Sqs.Shared;
using Microsoft.Extensions.Options;

namespace Holtz.Sqs.Application;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;

    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }


    private async ValueTask<string> GetQueueUrlAsync(CancellationToken cancellationToken)
    {
        if (_queueUrl is not null)
            return _queueUrl;

        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, cancellationToken);
        _queueUrl = queueUrlResponse.QueueUrl;
        return _queueUrl;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken)
    {
        string queueUrl = await GetQueueUrlAsync(cancellationToken);

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };

        return await _sqs.SendMessageAsync(sendMessageRequest, cancellationToken);
    }
}
