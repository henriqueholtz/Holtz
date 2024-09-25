using Holtz.Sqs.Application.DTOs;

namespace Holtz.Sqs.Application.Interfaces;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<CustomerDto?> GetAsync(Guid id);

    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
