using Holtz.DynamoDb.Domain;
using Holtz.DynamoDb.Domain.Interfaces;

namespace Holtz.DynamoDb.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    public Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
