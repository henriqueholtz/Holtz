using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
    {
        private readonly IProductsQueryRepository _repository;
        public GetProductQueryHandler(IProductsQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetProductByIdAsync(request.Id);
            if (product == null)
                return null;

            return new ProductDto(product);
        }
    }
}
