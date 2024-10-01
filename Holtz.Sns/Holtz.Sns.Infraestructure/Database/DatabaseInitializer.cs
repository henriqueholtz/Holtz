using Dapper;

namespace Holtz.Sns.Infraestructure.Database;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Customers (
        Id UUID PRIMARY KEY, 
        GitHubUsername TEXT NOT NULL,
        FullName TEXT NOT NULL,
        Email TEXT NOT NULL,
        BirthDate TEXT NOT NULL)");
    }
}