namespace Holtz.Sqs.Shared.Messages;

public class CustomerCreated
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string GithubUsername { get; init; }
    public required DateTime BirthDate { get; init; }
}
