namespace Holtz.S3.Api.Configurations;

public record S3Settings
{
    public const string Key = "AWS_S3";

    public required string BucketName { get; init; }
}
