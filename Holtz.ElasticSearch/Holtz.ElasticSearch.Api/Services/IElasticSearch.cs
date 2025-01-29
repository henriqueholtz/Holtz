using System;
using Holtz.ElasticSearch.Api.Models;

namespace Holtz.ElasticSearch.Api.Services;

public interface IElasticSearch
{
    Task CreateIndexIfNotExistsAsync(string indexName, CancellationToken cancellationToken);
    Task<bool> AddOrUpdate(User user, CancellationToken cancellationToken);
    Task<bool> AddOrUpdateBulk(IEnumerable<User> users, string indexName, CancellationToken cancellationToken);
    Task<User?> GetAsync(string key, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string key, CancellationToken cancellationToken);
    Task<long?> DeleteAllAsync(string key, CancellationToken cancellationToken);

}
