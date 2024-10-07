using Microsoft.AspNetCore.Mvc;

namespace Holtz.S3.Api.DTOs;

public record UpdateCustomerRequestDto
{
    [FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public CustomerRequestDto Customer { get; set; } = default!;

}
