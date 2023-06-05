using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.CQRS.Infraestructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Holtz.CQRS.Tests
{
    /// <summary>
    /// To make this method works, needs to install XUnit.DependencyInjection package
    /// </summary>
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).GetTypeInfo().Assembly));
            services.AddSingleton<IApplicationContext, ApplicationContext>();
        }
    }
}
