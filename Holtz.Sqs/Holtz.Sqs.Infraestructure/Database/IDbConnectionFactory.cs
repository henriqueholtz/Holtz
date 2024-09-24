using System.Data;

namespace Holtz.Sqs.Infraestructure.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}
