using Microsoft.AspNetCore.Mvc;

namespace Holtz.DynamoDb.Application.Dtos;

public record UpdateCustomerRequestDto
{
    [FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public CustomerRequestDto Customer { get; set; } = default!;
}
