using Holtz.Refit.Domain;
using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Net;

namespace Holtz.Refit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IRandomDataApi randomDataApi) : ControllerBase
    {
        private readonly IRandomDataApi _randomDataApi = randomDataApi;

        /// <summary>
        /// Get the list of users
        /// </summary>
        /// <param name="limit">Minimum supported by https://random-data-api.com/ 2.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(500, Type = typeof(ApiException))]
        public async Task<IActionResult> GetUsersAsync([FromQuery] int limit = 5)
        {
            ApiResponse<List<User>> response = await _randomDataApi.GetUsersAsync(limit);
            if (response.IsSuccessStatusCode)
                return Ok(response.Content);

            return StatusCode((int)HttpStatusCode.InternalServerError, response.Error);
        }
    }
}
