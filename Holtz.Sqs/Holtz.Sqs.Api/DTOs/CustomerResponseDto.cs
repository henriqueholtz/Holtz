namespace Holtz.Sqs.Api.DTOs;

public class CustomerResponseDto
{
    public Guid Id { get; init; }

    public string GitHubUsername { get; init; } = default!;

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateTime BirthDate { get; init; }
}
