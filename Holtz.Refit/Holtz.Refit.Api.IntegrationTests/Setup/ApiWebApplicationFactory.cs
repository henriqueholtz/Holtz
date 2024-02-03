using Holtz.Refit.Api.IntegrationTests.WireMockServers;
using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WireMock.Server;

namespace Holtz.Refit.Api.IntegrationTests.Setup
{
    public class ApiWebApplicationFactory : WebApplicationFactory<ProgramApiMarker>
    {
        public static WireMockServer RandomDataApiServer = WireMockServer.Start(RandomDataApiWireMockServer.DEFAULT_PORT);
        public WireMockServer GetRandomDataApiServer() => RandomDataApiServer;
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
        protected override void Dispose(bool disposing)
        {
            RandomDataApiServer.Stop();
            base.Dispose(disposing);
        }
    }
}
