using Holtz.CQRS.Application.Commands.AddProduct;
using Holtz.CQRS.Application.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.CQRS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<object>))]
        public async Task<IActionResult> Index([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(long))]
        public async Task<IActionResult> AddProduct([FromQuery] AddProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok();
        }
    }
}
