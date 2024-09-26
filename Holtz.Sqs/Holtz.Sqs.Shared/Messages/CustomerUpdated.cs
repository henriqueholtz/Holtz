using Holtz.Sqs.Shared.Interfaces;

namespace Holtz.Sqs.Shared.Messages;

public record CustomerUpdated : ISqsMessageMarker
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime BirthDate { get; init; }
}
