
using Dapper;
using Holtz.Sns.Domain;
using Holtz.Sns.Domain.Interfaces;
using Holtz.Sns.Infraestructure.Database;

namespace Holtz.Sns.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CustomerRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (Id, GitHubUsername, FullName, Email, BirthDate) 
            VALUES (@Id, @GitHubUsername, @FullName, @Email, @BirthDate)",
            customer).WaitAsync(cancellationToken);
        return result > 0;
    }

    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Customer>(
            "SELECT * FROM Customers WHERE Id = @Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<Customer>("SELECT * FROM Customers", cancellationToken);
    }

    public async Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        var result = await connection.ExecuteAsync(
            @"UPDATE Customers SET GitHubUsername = @GitHubUsername, FullName = @FullName, Email = @Email, 
                 BirthDate = @BirthDate WHERE Id = @Id",
            customer).WaitAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken).WaitAsync(cancellationToken);
        var result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }
}
