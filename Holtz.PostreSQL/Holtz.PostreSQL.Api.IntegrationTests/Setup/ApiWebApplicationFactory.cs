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
            .WithImage("postgres")
            .WithPassword("Holtz@Postgres!") //Same of Holtz.PostreSQL.Api/appsettings.json
            .Build();

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
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
            });
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
