using Holtz.PostreSQL.Api.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Holtz.PostreSQL.Api.IntegrationTests.Setup
{
    public class ApiWebApplicationFactory : WebApplicationFactory<ProgramApiMarker>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .Build();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync()
                .ConfigureAwait(false);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                var descriptorType = typeof(DbContextOptions<HoltzPostgreSqlContext>);
                var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<HoltzPostgreSqlContext>(options =>
                        options.UseNpgsql(_dbContainer.GetConnectionString()));

                // Ensure schema gets created
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<HoltzPostgreSqlContext>();
                context.Database.EnsureCreated();
            });
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.StopAsync()
                .ConfigureAwait(false);
        }
    }
}
