using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.Domain.Entities;
using Moq;

namespace Holtz.CQRS.Tests.Application.Queries
{
    public class GetProductsQueryHandlerTests
    {
        private readonly Mock<IProductsQueryRepository> _repositoryMock = new();
        public GetProductsQueryHandlerTests()
        {
            _repositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(new List<Product> { new Product("Mock", "Desc", 15) });
        }
        [Fact]
        public async Task Should_Call_Get_Method_Once()
        {
            var query = new GetProductsQuery();
            GetProductsQueryHandler handler = new GetProductsQueryHandler(_repositoryMock.Object);
            IList<ProductDto> products = await handler.Handle(query, default);

            _repositoryMock.Verify(x => x.GetProductsAsync(), Times.Once);
            Assert.True(products.Any());
        }
    }
}