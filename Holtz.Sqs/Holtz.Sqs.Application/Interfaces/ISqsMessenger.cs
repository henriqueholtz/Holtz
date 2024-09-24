using Amazon.SQS.Model;

namespace Holtz.Sqs.Application.Interfaces;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken);
}
