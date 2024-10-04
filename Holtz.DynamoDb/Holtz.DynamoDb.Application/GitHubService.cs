using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Holtz.DynamoDb.Application.Interfaces;

namespace Holtz.DynamoDb.Application;

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
