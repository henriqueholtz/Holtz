using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    /// <summary>
    /// <see cref="GetProductsQueryHandler"/>
    /// </summary>
    public class GetProductsQuery : IRequest<IList<Product>>
    {
    }
}
