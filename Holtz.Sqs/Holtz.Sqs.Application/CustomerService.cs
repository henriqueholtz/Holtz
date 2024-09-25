using Holtz.Sqs.Application.DTOs;
using Holtz.Sqs.Application.Interfaces;
using Holtz.Sqs.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Holtz.Sqs.Application.Mappings;
using Holtz.Sqs.Shared.Messages;

namespace Holtz.Sqs.Application;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    private readonly ISqsMessenger _sqsMessenger;

    public CustomerService(ICustomerRepository customerRepository,
        IGitHubService gitHubService, ISqsMessenger sqsMessenger)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        _sqsMessenger = sqsMessenger;
    }

    public async Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
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
        var response = await _customerRepository.CreateAsync(customer);
        if (response)
            await _sqsMessenger.SendMessageAsync(customer.ToCustomerCreatedMessage(), cancellationToken);

        return response;
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

    public async Task<bool> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var customer = customerDto.ToCustomer();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var response = await _customerRepository.UpdateAsync(customer);
        if (response)
            await _sqsMessenger.SendMessageAsync(customer.ToCustomerUpdatedMessage(), cancellationToken);

        return response;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _customerRepository.DeleteAsync(id);
        if (response)
            await _sqsMessenger.SendMessageAsync(
                new CustomerDeleted
                {
                    Id = id,
                },
                cancellationToken
            );
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
