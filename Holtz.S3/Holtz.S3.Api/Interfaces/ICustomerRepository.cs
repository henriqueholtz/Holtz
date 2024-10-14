using Holtz.S3.Api.Domain;

namespace Holtz.S3.Api.Interfaces;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken);

    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}