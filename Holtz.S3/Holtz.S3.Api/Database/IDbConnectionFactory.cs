using System.Data;

namespace Holtz.S3.Api.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}
