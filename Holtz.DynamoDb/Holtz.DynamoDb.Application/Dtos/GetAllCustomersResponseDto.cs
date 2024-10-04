namespace Holtz.DynamoDb.Application.Dtos;

public record GetAllCustomersResponseDto
{
    public IEnumerable<CustomerResponseDto> Customers { get; init; } = Enumerable.Empty<CustomerResponseDto>();
}
