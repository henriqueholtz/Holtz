using MediatR;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    /// <summary>
    /// <see cref="GetProductsQueryHandler"/>
    /// </summary>
    public class GetProductsQuery : IRequest<List<object>>
    {
    }
}
