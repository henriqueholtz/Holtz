using Microsoft.AspNetCore.Mvc;

namespace Holtz.Versioning.Api.Controllers.v3
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { Now = DateTime.Now, ApiVersion = "3.0" });
        }
    }
}