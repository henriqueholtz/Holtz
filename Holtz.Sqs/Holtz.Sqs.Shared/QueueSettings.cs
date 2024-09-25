namespace Holtz.Sqs.Shared;

public record QueueSettings
{
    public const string Key = "Queue";
    public required string Name { get; init; }
}
