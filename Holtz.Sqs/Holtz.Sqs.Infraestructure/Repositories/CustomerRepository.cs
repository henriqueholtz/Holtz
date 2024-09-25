using Dapper;
using Holtz.Sqs.Domain;
using Holtz.Sqs.Domain.Interfaces;
using Holtz.Sqs.Infraestructure.Database;

namespace Holtz.Sqs.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CustomerRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (Id, GitHubUsername, FullName, Email, BirthDate) 
            VALUES (@Id, @GitHubUsername, @FullName, @Email, @BirthDate)",
            customer);
        return result > 0;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<Customer>(
            "SELECT * FROM Customers WHERE Id = @Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Customer>("SELECT * FROM Customers");
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Customers SET GitHubUsername = @GitHubUsername, FullName = @FullName, Email = @Email, 
                 BirthDate = @BirthDate WHERE Id = @Id",
            customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }
}
