namespace Holtz.DynamoDb.Shared;

public record DynamoDbConfiguration
{
    public const string Key = "DynamoDb";

    public required string TableName { get; init; }
}
