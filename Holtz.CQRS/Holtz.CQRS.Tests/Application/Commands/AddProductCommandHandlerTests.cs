using Holtz.CQRS.Application.Commands.AddProduct;
using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;
using Moq;

namespace Holtz.CQRS.Tests.Application.Commands
{
    public class AddProductCommandHandlerTests
    {
        private readonly Mock<IProductsCommandRepository> _repositoryMock = new();
        public AddProductCommandHandlerTests()
        {
            _repositoryMock.Setup(x => x.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(Guid.NewGuid);
        }

        [Fact]
        public async Task Should_Call_Add_Method_Once()
        {
            AddProductCommand command = new AddProductCommand();
            AddProductCommandHandler handler = new AddProductCommandHandler(_repositoryMock.Object);
            Guid result = await handler.Handle(command, default);

            _repositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once);
            Assert.True(result != Guid.Empty);
        }
    }
}
