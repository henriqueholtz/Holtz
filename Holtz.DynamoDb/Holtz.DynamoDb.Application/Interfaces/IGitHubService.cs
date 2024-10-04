namespace Holtz.DynamoDb.Application.Interfaces;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUserAsync(string username, CancellationToken cancellationToken);
}
