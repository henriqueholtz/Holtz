using Holtz.Refit.Domain;
using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.Refit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IRandomDataApi randomDataApi) : ControllerBase
    {
        private readonly IRandomDataApi _randomDataApi = randomDataApi;

        /// <summary>
        /// Get the users list
        /// </summary>
        /// <param name="limit">Minimum supported by https://random-data-api.com/ 2.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] int limit = 5)
        {
            List<User> users = await _randomDataApi.GetUsersAsync(limit);
            return Ok(users);
        }
    }
}
