using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Interfaces
{
    public interface IProductsQueryRepository
    {
        Task<IList<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
    }
}
