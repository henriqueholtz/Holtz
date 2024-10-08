namespace Holtz.Sqs.Domain.Interfaces;

public interface ICustomerRepository
{
    // TODO: Add Cancellation Token
    Task<bool> CreateAsync(Customer customer);

    Task<Customer?> GetAsync(Guid id);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> UpdateAsync(Customer customer);

    Task<bool> DeleteAsync(Guid id);
}