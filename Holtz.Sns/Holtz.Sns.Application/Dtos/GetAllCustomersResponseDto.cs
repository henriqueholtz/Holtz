namespace Holtz.Sns.Application.Dtos;

public record class GetAllCustomersResponseDto
{
    public IEnumerable<CustomerResponseDto> Customers { get; init; } = Enumerable.Empty<CustomerResponseDto>();
}
