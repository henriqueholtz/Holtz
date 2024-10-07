namespace Holtz.S3.Api.DTOs;

public record CustomerDto
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string GitHubUsername { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required DateTime BirthDate { get; init; }
}
