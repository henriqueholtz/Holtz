using Holtz.Sns.Shared.Interfaces;

namespace Holtz.Sns.Shared.Messages;

public record CustomerCreated : ISnsMessageMarker
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime BirthDate { get; init; }
}
