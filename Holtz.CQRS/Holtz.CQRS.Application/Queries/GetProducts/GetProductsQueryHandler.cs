using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IList<Product>>
    {
        private readonly IProductsQueryRepository _repository;
        public GetProductsQueryHandler(IProductsQueryRepository repository)
        {
            _repository = repository;
        }
        public async Task<IList<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            // TODO: map to DTOs
            return await _repository.GetProductsAsync();
        }
    }
}
