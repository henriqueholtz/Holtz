using Holtz.CQRS.Infraestructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Holtz.CQRS.Api.IntegrationTests.Setup
{
    public class HoltzCqrsApiFactory : WebApplicationFactory<ProgramApiMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptorType = typeof(DbContextOptions<ApplicationContext>);
                var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlite("DataSource=:memory:"));

                // Ensure schema gets created
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationContext>();
                context.Database.EnsureCreated();

            });
        }
    }
}
