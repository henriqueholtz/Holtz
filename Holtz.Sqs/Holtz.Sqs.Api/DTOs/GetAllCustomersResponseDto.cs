namespace Holtz.Sqs.Api.DTOs;

public class GetAllCustomersResponseDto
{
    public IEnumerable<CustomerResponseDto> Customers { get; init; } = Enumerable.Empty<CustomerResponseDto>();
}
