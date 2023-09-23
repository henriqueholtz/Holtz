using Holtz.Catalog.Microservices.DAL.Entities;
using Holtz.Catalog.Microservices.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.Catalog.Microservices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public CatalogsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            return Ok(await _productRepository.GetAllAsync());
        }

        [HttpGet("{id:length(24)}", Name = "GetByIdAsync")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetByIdAsync(string id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("{categoryName}", Name = "GetByCategoryNameAsync")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return BadRequest();

            IEnumerable<Product> products = await _productRepository.GetByCategoryNameAsync(categoryName);
            return Ok(products);
        }

        [HttpPost(Name = "CreateProductAsync")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateProductAsync([FromBody] Product product)
        {
            if (product is null)
                return BadRequest("Invalid Product!");

            await _productRepository.CreateProductAsync(product);
            return CreatedAtRoute("GetByIdAsync", new { id = product.Id }, product);
        }

        [HttpPut(Name = "UpdateProductAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] Product product)
        {
            if (product is null)
                return BadRequest("Invalid Product!");

            bool result = await _productRepository.UpdateProductAsync(product);
            if (result)
                return Ok(result);

            return BadRequest("An error ocurred when updating the product.");
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProductAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProductAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Invalid id!");

            bool result = await _productRepository.DeleteProductAsync(id);
            if (result)
                return Ok(result);

            return BadRequest("An error ocurred when removing the product.");
        }
    }
}
