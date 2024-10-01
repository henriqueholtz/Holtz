using Holtz.Sns.Shared.Interfaces;

namespace Holtz.Sns.Shared.Messages;

public record class CustomerDeleted : ISnsMessageMarker
{
    public required Guid Id { get; init; }
}
