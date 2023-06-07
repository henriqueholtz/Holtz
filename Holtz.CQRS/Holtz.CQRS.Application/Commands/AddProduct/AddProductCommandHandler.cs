using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;
using MediatR;

namespace Holtz.CQRS.Application.Commands.AddProduct
{
    internal class AddProductCommandHandler : IRequestHandler<AddProductCommand, Guid>
    {
        private readonly IProductsCommandRepository _repository;
        public AddProductCommandHandler(IProductsCommandRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddProductAsync(new Product(request.Name, request.Description, request.Price));
        }
    }
}
