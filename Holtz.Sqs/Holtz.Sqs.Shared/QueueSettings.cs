namespace Holtz.Sqs.Shared;

public record QueueSettings
{
    public required string QueueName { get; init; }
}
