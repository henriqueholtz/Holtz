namespace Holtz.Sqs.Application.Interfaces;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}
