using Microsoft.AspNetCore.Mvc;

namespace Holtz.Versioning.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Health()
        {
            return Ok(DateTime.Now);
        }
    }
}