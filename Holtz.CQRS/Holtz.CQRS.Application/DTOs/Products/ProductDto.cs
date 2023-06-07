using Holtz.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Holtz.CQRS.Application.DTOs.Products
{
    public class ProductDto
    {
        public ProductDto(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Price = product.Price;
            this.Description = product.Description;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
