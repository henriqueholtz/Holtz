using Holtz.CQRS.Application.DTOs.Products;

namespace Holtz.CQRS.Application.Queries.GetProduct
{
    /// <summary>
    /// <see cref="GetProductQueryHandler"/>
    /// </summary>
    public class GetProductQuery : IRequest<ProductDto?>
    {
        public Guid Id { get; set; }
    }
}
