namespace Holtz.Sns.Application.Interfaces;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUserAsync(string username);
}
