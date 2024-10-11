using System.Net;
using Amazon.S3;
using Holtz.S3.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.S3.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerImageController : ControllerBase
    {
        private readonly ICustomerImageService _customerImageService;

        public CustomerImageController(ICustomerImageService customerImageService)
        {
            _customerImageService = customerImageService;
        }

        [HttpPost("customers/{id:guid}/image")]
        public async Task<IActionResult> UploadAsync([FromRoute] Guid id,
            [FromForm(Name = "Data")] IFormFile file, CancellationToken cancellationToken)
        {
            var response = await _customerImageService.UploadImageAsync(id, file, cancellationToken);
            if (response.HttpStatusCode == HttpStatusCode.OK)
                return Ok();

            return BadRequest();
        }

        [HttpGet("customers/{id:guid}/image")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _customerImageService.GetImageAsync(id, cancellationToken);
                return File(response.ResponseStream, response.Headers.ContentType);
            }
            catch (AmazonS3Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("customers/{id:guid}/image")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
