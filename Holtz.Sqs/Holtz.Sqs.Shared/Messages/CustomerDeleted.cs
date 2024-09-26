using Holtz.Sqs.Shared.Interfaces;

namespace Holtz.Sqs.Shared.Messages;

public record CustomerDeleted : ISqsMessageMarker
{
    public required Guid Id { get; init; }
}
