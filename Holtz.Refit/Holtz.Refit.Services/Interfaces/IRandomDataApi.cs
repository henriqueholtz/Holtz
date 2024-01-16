using Holtz.Refit.Domain;
using Refit;
using System.Net.Mime;

namespace Holtz.Refit.Services.Interfaces
{
    [Headers($"accept: {MediaTypeNames.Application.Json}")]
    public interface IRandomDataApi
    {
        [Get("/api/v2/users?size={size}")]
        Task<ApiResponse<List<User>>> GetUsersAsync([AliasAs("size")] int limit = 5);

        [Get("/api/v2/beers?size={limit}")]
        Task<ApiResponse<List<Beer>>> GetBeersAsync(int limit = 5);
    }
}
