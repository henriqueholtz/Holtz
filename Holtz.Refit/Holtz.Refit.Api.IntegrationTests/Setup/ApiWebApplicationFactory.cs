using Holtz.Refit.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Server;

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
                    c.BaseAddress = new Uri("http://localhost:9876");
                    c.Timeout = TimeSpan.FromMilliseconds(1 * 1000);
                });
            });
        }
    }
}
