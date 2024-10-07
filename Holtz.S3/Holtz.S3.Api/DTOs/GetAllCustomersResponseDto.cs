namespace Holtz.S3.Api.DTOs;

public record GetAllCustomersResponseDto
{
    public IEnumerable<CustomerResponseDto> Customers { get; init; } = Enumerable.Empty<CustomerResponseDto>();
}
