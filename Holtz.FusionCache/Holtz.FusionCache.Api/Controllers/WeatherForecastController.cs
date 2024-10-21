using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace Holtz.FusionCache.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFusionCache _fusionCache;
        private readonly ILogger<WeatherForecastController> _logger;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFusionCache fusionCache)
        {
            _logger = logger;
            _fusionCache = fusionCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string cacheKey = "test", bool skipCache = false)
        {
            if (skipCache)
                await _fusionCache.RemoveAsync(cacheKey);

            return _fusionCache.GetOrSet<IEnumerable<WeatherForecast>>(
                cacheKey, (ct) =>
                {
                    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();
                },
                options => options.DistributedCacheDuration = TimeSpan.FromMinutes(10)
            );
        }
    }
}
