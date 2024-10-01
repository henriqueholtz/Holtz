using Microsoft.AspNetCore.Mvc;

namespace Holtz.Sns.Application.Dtos;

public record class UpdateCustomerRequestDto
{
    [FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public CustomerRequestDto Customer { get; set; } = default!;
}
