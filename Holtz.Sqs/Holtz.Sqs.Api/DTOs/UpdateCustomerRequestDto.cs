using Microsoft.AspNetCore.Mvc;

namespace Holtz.Sqs.Api.DTOs;

public class UpdateCustomerRequestDto
{
    [FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public CustomerRequestDto Customer { get; set; } = default!;
}

