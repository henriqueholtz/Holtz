namespace Holtz.Sqs.Application.DTOs;

public class GetAllCustomersResponseDto
{
    public IEnumerable<CustomerResponseDto> Customers { get; init; } = Enumerable.Empty<CustomerResponseDto>();
}
