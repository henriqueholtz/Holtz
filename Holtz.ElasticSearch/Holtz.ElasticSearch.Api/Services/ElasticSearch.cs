using Elastic.Clients.Elasticsearch;
using Holtz.ElasticSearch.Api.Configurations;
using Holtz.ElasticSearch.Api.Models;
using Microsoft.Extensions.Options;

namespace Holtz.ElasticSearch.Api.Services;

public class ElasticSearch : IElasticSearch
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private readonly ElasticSettings _elasticSettings;

    public ElasticSearch(IOptions<ElasticSettings> optionsMonitor)
    {
        _elasticSettings = optionsMonitor.Value;
        var settings = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
            // .Authentication()
            .DefaultIndex(_elasticSettings.DefaultIndex);

        _elasticsearchClient = new ElasticsearchClient(settings);
    }

    public async Task<bool> AddOrUpdate(User user, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.IndexAsync<User>(
            user,
            idx => idx.Index(_elasticSettings.DefaultIndex).OpType(OpType.Index)
        );

        return response.IsValidResponse;
    }

    public async Task<bool> AddOrUpdateBulk(IEnumerable<User> users, string indexName, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.BulkAsync(b =>
            b.Index(_elasticSettings.DefaultIndex).UpdateMany<User>(
                users,
                (ud, u) => ud.Doc(u).DocAsUpsert(true)
            )
        );

        return response.IsValidResponse;
    }

    public async Task CreateIndexIfNotExistsAsync(string indexName, CancellationToken cancellationToken)
    {
        if (!(await _elasticsearchClient.Indices.ExistsAsync(indexName, cancellationToken)).Exists)
            await _elasticsearchClient.CreateAsync(indexName);
    }

    public async Task<long?> DeleteAllAsync(string key, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.DeleteByQueryAsync<User>(d => d.Indices(_elasticSettings.DefaultIndex), cancellationToken);

        if (response.IsValidResponse)
            return response.Deleted;

        return default;
    }

    public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.DeleteAsync<User>(key, d => d.Index(_elasticSettings.DefaultIndex), cancellationToken);
        return response.IsValidResponse;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.SearchAsync<User>(s => s.Index(_elasticSettings.DefaultIndex));

        if (response.IsValidResponse)
            return response.Documents.ToList();

        return new List<User>();
    }

    public async Task<User?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.GetAsync<User>(key, g => g.Index(_elasticSettings.DefaultIndex), cancellationToken);
        return response.Source;
    }
}
