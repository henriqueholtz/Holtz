using Holtz.S3.Api.Domain;
using Holtz.S3.Api.DTOs;

namespace Holtz.S3.Api.Mappings;

public static class Mappers
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            BirthDate = customer.BirthDate
        };
    }

    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            BirthDate = customerDto.BirthDate
        };
    }

    public static CustomerDto ToCustomerDto(this CustomerRequestDto request)
    {
        return new CustomerDto
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            GitHubUsername = request.GitHubUsername,
            FullName = request.FullName,
            BirthDate = request.BirthDate
        };
    }

    public static CustomerDto ToCustomerDto(this UpdateCustomerRequestDto request)
    {
        return new CustomerDto
        {
            Id = request.Id,
            Email = request.Customer.Email,
            GitHubUsername = request.Customer.GitHubUsername,
            FullName = request.Customer.FullName,
            BirthDate = request.Customer.BirthDate
        };
    }

    public static CustomerResponseDto ToCustomerResponseDto(this CustomerDto customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            BirthDate = customer.BirthDate
        };
    }

    public static GetAllCustomersResponseDto ToCustomersResponseDto(this IEnumerable<CustomerDto> customersDto)
    {
        return new GetAllCustomersResponseDto
        {
            Customers = customersDto.Select(x => new CustomerResponseDto
            {
                Id = x.Id,
                Email = x.Email,
                GitHubUsername = x.GitHubUsername,
                FullName = x.FullName,
                BirthDate = x.BirthDate
            })
        };
    }
}
