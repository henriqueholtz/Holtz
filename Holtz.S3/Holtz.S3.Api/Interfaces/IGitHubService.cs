namespace Holtz.S3.Api.Interfaces;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUserAsync(string username, CancellationToken cancellationToken);
}
