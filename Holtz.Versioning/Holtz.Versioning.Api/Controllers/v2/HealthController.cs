using Microsoft.AspNetCore.Mvc;

namespace Holtz.Versioning.Api.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { Now = DateTime.Now, ApiVersion = "2.0" });
        }
    }
}