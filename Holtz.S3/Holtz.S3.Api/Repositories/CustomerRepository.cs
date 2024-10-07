using Holtz.S3.Api.Domain;
using Holtz.S3.Api.Interfaces;

namespace Holtz.S3.Api.Repositories;

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
