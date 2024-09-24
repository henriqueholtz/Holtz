using Holtz.Sqs.Application.DTOs;

namespace Holtz.Sqs.Application.Interfaces;

public interface ICustomerService
{
    Task<bool> CreateAsync(CustomerDto customer);

    Task<CustomerDto?> GetAsync(Guid id);

    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<bool> UpdateAsync(CustomerDto customer);

    Task<bool> DeleteAsync(Guid id);
}
