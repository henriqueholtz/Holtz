namespace Holtz.ElasticSearch.Api.Configurations;

public record ElasticSettings
{
    public required string Url { get; set; } = string.Empty;
    public required string DefaultIndex { get; set; } = string.Empty;
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}
