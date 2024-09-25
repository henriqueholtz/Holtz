namespace Holtz.Sqs.Application.Interfaces;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUserAsync(string username);
}
