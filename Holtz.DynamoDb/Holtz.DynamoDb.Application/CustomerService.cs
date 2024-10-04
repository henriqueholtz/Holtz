using FluentValidation;
using FluentValidation.Results;
using Holtz.DynamoDb.Application.Dtos;
using Holtz.DynamoDb.Application.Interfaces;
using Holtz.DynamoDb.Application.Mappings;
using Holtz.DynamoDb.Domain.Interfaces;

namespace Holtz.DynamoDb.Application;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;

    public CustomerService(ICustomerRepository customerRepository, IGitHubService gitHubService)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
    }
    public async Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var existingUser = await _customerRepository.GetAsync(customerDto.Id, cancellationToken);
        if (existingUser is not null)
        {
            var message = $"A user with id {customerDto.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(CustomerDto), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customerDto.GitHubUsername, cancellationToken);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customerDto.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customerDto.GitHubUsername), message));
        }

        var customer = customerDto.ToCustomer();
        var response = await _customerRepository.CreateAsync(customer, cancellationToken);

        return response;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _customerRepository.DeleteAsync(id, cancellationToken);
        return response;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
        return customers.Select(c => c.ToCustomerDto());
    }

    public async Task<CustomerDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(id, cancellationToken);
        return customer?.ToCustomerDto();
    }

    public async Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var customer = customerDto.ToCustomer();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customer.GitHubUsername, cancellationToken);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var response = await _customerRepository.UpdateAsync(customer, cancellationToken);

        return response;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}
