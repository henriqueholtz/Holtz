using Holtz.CQRS.Api.Controllers;
using Holtz.CQRS.Application.Commands.AddProduct;
using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Holtz.CQRS.Tests.Api
{
    /// <summary>
    /// https://www.learmoreseekmore.com/2022/02/dotnet6-unit-testing-aspnetcore-web-api-using-xunit.html
    /// </summary>
    public class ProductsControllerTests
    {
        private readonly IMediator _mediator;
        public ProductsControllerTests(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Fact]
        public async Task Should_GetProductsAsync()
        {
            // Arrange
            ProductsController? controller = new ProductsController(_mediator);

            // Act
            OkObjectResult result = (OkObjectResult)await controller.Index(new GetProductsQuery());

            // Assert
            Assert.True(result.Value is IList<ProductDto>);
            Assert.True(result.StatusCode == 200);
        }

        [Fact]
        public async Task Should_AddProductAsync()
        {
            // Arrange
            ProductsController? controller = new ProductsController(_mediator);

            // Act
            OkObjectResult result = (OkObjectResult)await controller.AddProductAsync(new AddProductCommand { Name = "Product 1", Description = "Description", Price = 8 });

            // Assert
            Assert.True(result.Value is Guid);
            Assert.True(result.StatusCode == 200);
        }
    }
}
