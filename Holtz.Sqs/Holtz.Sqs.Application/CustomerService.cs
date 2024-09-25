using Holtz.Sqs.Application.DTOs;
using Holtz.Sqs.Application.Interfaces;
using Holtz.Sqs.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Holtz.Sqs.Application.Mappings;

namespace Holtz.Sqs.Application;

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

    public async Task<bool> CreateAsync(CustomerDto customerDto)
    {
        var existingUser = await _customerRepository.GetAsync(customerDto.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customerDto.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(CustomerDto), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customerDto.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customerDto.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customerDto.GitHubUsername), message));
        }

        var customer = customerDto.ToCustomer();
        return await _customerRepository.CreateAsync(customer);
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(id);
        return customer?.ToCustomerDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => c.ToCustomerDto());
    }

    public async Task<bool> UpdateAsync(CustomerDto customerDto)
    {
        var customer = customerDto.ToCustomer();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customer.GithubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GithubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GithubUsername), message));
        }

        return await _customerRepository.UpdateAsync(customer);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}