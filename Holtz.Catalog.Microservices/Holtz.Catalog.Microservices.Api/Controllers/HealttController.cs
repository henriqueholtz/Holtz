using Microsoft.AspNetCore.Mvc;

namespace Holtz.Catalog.Microservices.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealttController : ControllerBase
    {

        [HttpGet(Name = "HealthCheck")]
        public IActionResult Health()
        {
            return Ok(DateTime.Now);
        }
    }
}