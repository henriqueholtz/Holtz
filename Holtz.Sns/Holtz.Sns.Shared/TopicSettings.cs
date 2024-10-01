namespace Holtz.Sns.Shared;

public record TopicSettings
{
    public const string Key = "Topic";

    public required string Name { get; init; }
}
