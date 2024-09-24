using Holtz.Sqs.Api.Attributes;
using Holtz.Sqs.Api.DTOs;
using Holtz.Sqs.Api.Mappings;
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

    [HttpPost("customers")]
    public async Task<IActionResult> Create([FromBody] CustomerRequestDto request)
    {
        var customer = request.ToCustomerDto();

        await _customerService.CreateAsync(customer);

        var customerResponse = customer.ToCustomerResponseDto();

        return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
    }

    [HttpGet("customers/{id:guid}")]
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

    [HttpGet("customers")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        var customersResponse = customers.ToCustomersResponseDto();
        return Ok(customersResponse);
    }

    [HttpPut("customers/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromMultiSource] UpdateCustomerRequestDto request)
    {
        var existingCustomer = await _customerService.GetAsync(request.Id);

        if (existingCustomer is null)
        {
            return NotFound();
        }

        var customer = request.ToCustomerDto();
        await _customerService.UpdateAsync(customer);

        var customerResponse = customer.ToCustomerResponseDto();
        return Ok(customerResponse);
    }

    [HttpDelete("customers/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _customerService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return Ok();
    }
}