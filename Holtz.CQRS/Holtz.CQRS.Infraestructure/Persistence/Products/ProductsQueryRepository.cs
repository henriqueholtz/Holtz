using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holtz.CQRS.Infraestructure.Persistence.Products
{
    public class ProductsQueryRepository : IProductsQueryRepository
    {
        private readonly ApplicationContext _context;
        public ProductsQueryRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IList<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
