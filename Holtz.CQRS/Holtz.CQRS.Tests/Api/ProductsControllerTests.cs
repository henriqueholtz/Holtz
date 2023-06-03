using Holtz.CQRS.Api.Controllers;
using Holtz.CQRS.Application.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Holtz.CQRS.Tests.Api
{
    /// <summary>
    /// https://www.learmoreseekmore.com/2022/02/dotnet6-unit-testing-aspnetcore-web-api-using-xunit.html
    /// </summary>
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProducts()
        {
            // Arrange
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).GetTypeInfo().Assembly));
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IMediator mediator = serviceProvider.GetService<IMediator>();
            ProductsController? controller = new ProductsController(mediator);

            // Act
            OkObjectResult result = (OkObjectResult)await controller.Index(new GetProductsQuery());


            // Assert
            Assert.True(result.StatusCode == 200);
        }
    }
}
