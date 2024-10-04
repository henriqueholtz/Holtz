using System.Data;

namespace Holtz.Sns.Infraestructure.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}
