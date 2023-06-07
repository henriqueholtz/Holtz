using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Interfaces
{
    public interface IProductsCommandRepository
    {
        Task<Guid> AddProductAsync(Product product); 
    }
}
