
using Dapper;

namespace Holtz.S3.Api.Database;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        using var connection = await _connectionFactory.CreateConnectionAsync(cts.Token);
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Customers (
            Id UUID PRIMARY KEY, 
            GitHubUsername TEXT NOT NULL,
            FullName TEXT NOT NULL,
            Email TEXT NOT NULL,
            BirthDate TEXT NOT NULL)"
        );
    }
}
