using Microsoft.AspNetCore.Mvc;

namespace Holtz.Versioning.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { Now = DateTime.Now, ApiVersion = "1.0" });
        }
    }
}