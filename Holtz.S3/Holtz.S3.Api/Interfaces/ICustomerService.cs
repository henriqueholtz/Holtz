using Holtz.S3.Api.DTOs;

namespace Holtz.S3.Api.Interfaces;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<CustomerDto?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
