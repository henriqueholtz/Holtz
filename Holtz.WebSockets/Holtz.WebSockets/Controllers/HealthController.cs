using Microsoft.AspNetCore.Mvc;

namespace Holtz.WebSockets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
       
        [HttpGet(Name = "HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok(DateTime.Now);
        }
    }
}