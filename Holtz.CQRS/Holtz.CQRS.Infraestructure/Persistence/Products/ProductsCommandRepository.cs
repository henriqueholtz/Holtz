using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Holtz.CQRS.Infraestructure.Persistence.Products
{
    public class ProductsCommandRepository : IProductsCommandRepository
    {
        private readonly ApplicationContext _context;
        public ProductsCommandRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddProductAsync(Product product)
        {
            EntityEntry<Product> result = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }
    }
}
