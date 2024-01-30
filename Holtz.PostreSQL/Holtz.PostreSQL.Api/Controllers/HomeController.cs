using Microsoft.AspNetCore.Mvc;

namespace Holtz.PostreSQL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Home()
        {
            return Ok("Holtz.PostreSQL.Api");
        }
    }
}
