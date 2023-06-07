using Holtz.CQRS.Application.Commands.AddProduct;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.Domain.Entities;
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
        
        /// <summary>
        /// Get list of products
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        public async Task<IActionResult> Index([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<IActionResult> AddProduct([FromQuery] AddProductCommand command)
        {
            Guid result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
