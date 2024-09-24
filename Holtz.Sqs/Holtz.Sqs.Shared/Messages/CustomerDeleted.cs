namespace Holtz.Sqs.Shared.Messages;

public record CustomerDeleted
{
    public required Guid Id { get; init; }
}
