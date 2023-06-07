using Holtz.CQRS.Application.DTOs.Products;

namespace Holtz.CQRS.Application.Queries.GetProducts
{
    /// <summary>
    /// <see cref="GetProductsQueryHandler"/>
    /// </summary>
    public class GetProductsQuery : IRequest<IList<ProductDto>>
    {
    }
}
