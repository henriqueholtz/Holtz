using Holtz.Sns.Api.Attributes;
using Holtz.Sns.Application.Dtos;
using Holtz.Sns.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Holtz.Sns.Application.Mappings;

namespace Holtz.Sns.Api.Controllers
{
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
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetAsync(id, cancellationToken);

            if (customer is null)
            {
                return NotFound();
            }

            var customerResponse = customer.ToCustomerResponseDto();
            return Ok(customerResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllAsync(cancellationToken);
            var customersResponse = customers.ToCustomersResponseDto();
            return Ok(customersResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromMultiSource] UpdateCustomerRequestDto request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerService.GetAsync(request.Id, cancellationToken);

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
}