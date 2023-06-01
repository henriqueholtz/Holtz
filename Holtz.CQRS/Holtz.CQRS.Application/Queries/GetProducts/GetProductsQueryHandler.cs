using MediatR;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<object>>
    {
        public Task<List<object>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<object> { new { Id = 1, Name = "Mock" } });
        }
    }
}
