using Holtz.Refit.Api.IntegrationTests.WireMockServers;
using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Holtz.Refit.Api.IntegrationTests.Setup
{
    public class ApiWebApplicationFactory : WebApplicationFactory<ProgramApiMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddRefitClient<IRandomDataApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri($"http://localhost:{RandomDataApiWireMockServer.DEFAULT_PORT}");
                    c.Timeout = TimeSpan.FromMilliseconds(1 * 1000);
                });
            });
        }
    }
}
