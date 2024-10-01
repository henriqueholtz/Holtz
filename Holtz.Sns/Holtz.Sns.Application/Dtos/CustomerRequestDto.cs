namespace Holtz.Sns.Application.Dtos;

public record class CustomerRequestDto
{
    public string GitHubUsername { get; init; } = default!;

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateTime BirthDate { get; init; } = default!;
}
