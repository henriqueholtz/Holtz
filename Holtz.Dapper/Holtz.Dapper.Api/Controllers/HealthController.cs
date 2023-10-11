using Microsoft.AspNetCore.Mvc;

namespace Holtz.Dapper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Health")]
        public IActionResult Get()
        {
            return Ok(DateTime.Now);
        }
    }
}