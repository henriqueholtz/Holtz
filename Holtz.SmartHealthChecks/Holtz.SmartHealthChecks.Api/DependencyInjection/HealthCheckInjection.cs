using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Holtz.SmartHealthChecks.Api.DependencyInjection
{
    public static class HealthCheckInjection
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks();

            services.AddHealthChecks()
                .AddSqlServer(connectionString, healthQuery: "select 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" });
        }
    }
}
