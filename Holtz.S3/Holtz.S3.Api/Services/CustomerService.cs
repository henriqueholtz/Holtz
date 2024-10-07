using FluentValidation;
using FluentValidation.Results;
using Holtz.S3.Api.DTOs;
using Holtz.S3.Api.Interfaces;
using Holtz.S3.Api.Mappings;

namespace Holtz.S3.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;

    public CustomerService(ICustomerRepository customerRepository,
        IGitHubService gitHubService)
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
            throw new ValidationException(message, GenerateValidationError(nameof(customerDto), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customerDto.GitHubUsername, cancellationToken);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customerDto.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customerDto.GitHubUsername), message));
        }

        return await _customerRepository.CreateAsync(customerDto.ToCustomer(), cancellationToken);
    }

    public async Task<CustomerDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(id, cancellationToken);
        return customer?.ToCustomerDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customerDtos = await _customerRepository.GetAllAsync(cancellationToken);
        return customerDtos.Select(c => c.ToCustomerDto());
    }

    public async Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customerDto.GitHubUsername, cancellationToken);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customerDto.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customerDto.GitHubUsername), message));
        }

        return await _customerRepository.UpdateAsync(customerDto.ToCustomer(), cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _customerRepository.DeleteAsync(id, cancellationToken);
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}
