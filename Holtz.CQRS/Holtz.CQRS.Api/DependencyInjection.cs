using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Infraestructure.Persistence.Products;

namespace Holtz.CQRS.Api
{
    public static class DependencyInjection
    {
        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProductsQueryRepository, ProductsQueryRepository>();
            services.AddTransient<IProductsCommandRepository, ProductsCommandRepository>();
        }
    }
}
