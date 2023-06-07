using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Interfaces
{
    public interface IProductsQueryRepository
    {
        Task<IList<Product>> GetProductsAsync(); 
    }
}
