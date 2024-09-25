using Holtz.Sqs.Api.Attributes;
using Holtz.Sqs.Application.DTOs;
using Holtz.Sqs.Application.Mappings;
using Holtz.Sqs.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.Sqs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerRequestDto request, CancellationToken cancellationToken)
    {
        var customer = request.ToCustomerDto();

        await _customerService.CreateAsync(customer, cancellationToken);

        var customerResponse = customer.ToCustomerResponseDto();

        return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var customer = await _customerService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        var customerResponse = customer.ToCustomerResponseDto();
        return Ok(customerResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        var customersResponse = customers.ToCustomersResponseDto();
        return Ok(customersResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromMultiSource] UpdateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var existingCustomer = await _customerService.GetAsync(request.Id);

        if (existingCustomer is null)
        {
            return NotFound();
        }

        var customer = request.ToCustomerDto();
        await _customerService.UpdateAsync(customer, cancellationToken);

        var customerResponse = customer.ToCustomerResponseDto();
        return Ok(customerResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return Ok();
    }
}
