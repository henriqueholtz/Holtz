using Microsoft.AspNetCore.Mvc;

namespace Holtz.Dapper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet(Name = "Health")]
        public IActionResult Get()
        {
            return Ok(DateTime.Now);
        }
    }
}