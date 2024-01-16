using Holtz.Refit.Domain;
using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Holtz.Refit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController(IRandomDataApi randomDataApi) : ControllerBase
    {
        private readonly IRandomDataApi _randomDataApi = randomDataApi;

        /// <summary>
        /// Get the list of beers
        /// </summary>
        /// <param name="limit">Minimum supported by https://random-data-api.com/ 2.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBeersAsync([FromQuery] int limit = 5)
        {
            List<Beer> beers = await _randomDataApi.GetBeersAsync(limit);
            return Ok(beers);
        }
    }
}
