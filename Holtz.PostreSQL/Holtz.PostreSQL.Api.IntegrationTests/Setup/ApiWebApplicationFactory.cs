using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Holtz.PostreSQL.Api.IntegrationTests.Setup
{
    public class ApiWebApplicationFactory : WebApplicationFactory<ProgramApiMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {

            });
        }
    }
}
