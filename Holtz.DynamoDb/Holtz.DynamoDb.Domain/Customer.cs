using System.Text.Json.Serialization;

namespace Holtz.DynamoDb.Domain;

public class Customer
{
    [JsonPropertyName("pk")]
    public string Pk => Id.ToString();

    [JsonPropertyName("sk")]
    public string Sk => Id.ToString();

    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime BirthDate { get; init; }

    public DateTime UpdatedAt { get; set; }
}
