using Holtz.Sns.Application.Dtos;

namespace Holtz.Sns.Application.Interfaces;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<CustomerDto?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
