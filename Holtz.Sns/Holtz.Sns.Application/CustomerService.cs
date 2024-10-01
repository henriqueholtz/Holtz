using System;
using FluentValidation;
using FluentValidation.Results;
using Holtz.Sns.Application.Dtos;
using Holtz.Sns.Application.Interfaces;
using Holtz.Sns.Application.Mappings;
using Holtz.Sns.Domain.Interfaces;
using Holtz.Sns.Shared.Messages;

namespace Holtz.Sns.Application;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    private readonly ISnsMessenger _snsMessenger;

    public CustomerService(ICustomerRepository customerRepository,
        IGitHubService gitHubService, ISnsMessenger snsMessenger)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        _snsMessenger = snsMessenger;
    }

    public async Task<bool> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var existingUser = await _customerRepository.GetAsync(customerDto.Id, cancellationToken);
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
        var response = await _customerRepository.CreateAsync(customer, cancellationToken);
        if (response)
            await _snsMessenger.SendMessageAsync(customer.ToCustomerCreatedMessage(), cancellationToken);

        return response;
    }

    public async Task<CustomerDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(id, cancellationToken);
        return customer?.ToCustomerDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
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

        var response = await _customerRepository.UpdateAsync(customer, cancellationToken);
        if (response)
            await _snsMessenger.SendMessageAsync(customer.ToCustomerUpdatedMessage(), cancellationToken);

        return response;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _customerRepository.DeleteAsync(id, cancellationToken);
        if (response)
            await _snsMessenger.SendMessageAsync(
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
