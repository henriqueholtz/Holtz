using Amazon.SimpleNotificationService.Model;

namespace Holtz.Sns.Application.Interfaces;

public interface ISnsMessenger
{
    Task<PublishResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken);
}
