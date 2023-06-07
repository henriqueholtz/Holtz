using Holtz.CQRS.Api.Controllers;
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
        private readonly Mock<IProductsQueryRepository> _repositoryMock = new();
        public ProductsControllerTests(IMediator mediator)
        {
            _mediator = mediator;
            _repositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(new List<Product> { new Product("Product 1", "Desc 1", 15) });
        }

        [Fact]
        public async Task GetProductsAsync()
        {
            // Arrange
            ProductsController? controller = new ProductsController(_mediator);

            // Act
            OkObjectResult result = (OkObjectResult)await controller.Index(new GetProductsQuery());

            // Assert
            Assert.True(result.Value is IList<ProductDto>);
            Assert.True(result.StatusCode == 200);
        }
    }
}
