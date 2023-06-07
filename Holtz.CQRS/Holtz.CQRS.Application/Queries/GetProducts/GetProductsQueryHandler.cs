using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IList<ProductDto>>
    {
        private readonly IProductsQueryRepository _repository;
        public GetProductsQueryHandler(IProductsQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            IList<Product> products = await _repository.GetProductsAsync();
            return products.Select(p => new ProductDto(p)).ToList();
        }
    }
}
