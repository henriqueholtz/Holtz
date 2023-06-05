using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IList<Product>>
    {
        private readonly IApplicationContext _context;
        public GetProductsQueryHandler(IApplicationContext context)
        {
            _context = context;
        }
        public Task<IList<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Products);
        }
    }
}
