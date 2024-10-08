using System.Net;
using System.Text.Json.Nodes;
using Holtz.S3.Api.Interfaces;

namespace Holtz.S3.Api.Services;

public class GitHubService : IGitHubService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> IsValidGitHubUserAsync(string username, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("GitHub");
        var response = await client.GetAsync($"/users/{username}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<JsonObject>();
            var message = responseBody!["message"]!.ToString();
            throw new HttpRequestException(message);
        }

        return response.StatusCode == HttpStatusCode.OK;
    }
}
