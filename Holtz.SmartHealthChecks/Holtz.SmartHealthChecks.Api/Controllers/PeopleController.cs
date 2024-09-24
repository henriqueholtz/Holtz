using Holtz.SmartHealthChecks.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holtz.SmartHealthChecks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController(HoltzSmartHealthChecksContext context) : ControllerBase
    {
        private readonly HoltzSmartHealthChecksContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetPeopleAsync()
        {
            var people = await _context.People.ToListAsync();
            return Ok(people);
        }
    }
}
