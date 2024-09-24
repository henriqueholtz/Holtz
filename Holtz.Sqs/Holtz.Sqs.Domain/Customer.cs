using System;

namespace Holtz.Sqs.Domain;

public class Customer
{
    public required Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string GithubUsername { get; init; }
    public required DateTime BirthDate { get; init; }
}
