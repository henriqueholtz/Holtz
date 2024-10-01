using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Holtz.Sns.Application.Interfaces;
using Holtz.Sns.Shared;
using Microsoft.Extensions.Options;

namespace Holtz.Sns.Application;

public class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly IOptions<TopicSettings> _topicSettings;

    private string? _topicArn;

    public SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
    {
        _sns = sns;
        _topicSettings = topicSettings;
    }

    private async ValueTask<string> GetTopicArnAsync()
    {
        if (_topicArn is not null)
            return _topicArn;

        var topicResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
        _topicArn = topicResponse.TopicArn;
        return _topicArn;
    }

    public async Task<PublishResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken)
    {
        string topicArn = await GetTopicArnAsync();

        var publishRequest = new PublishRequest
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
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

        return await _sns.PublishAsync(publishRequest, cancellationToken);
    }
}
