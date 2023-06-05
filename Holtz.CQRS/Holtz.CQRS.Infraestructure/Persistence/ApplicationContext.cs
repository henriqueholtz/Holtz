using Holtz.CQRS.Application.Interfaces;
using Holtz.Domain.Entities;

namespace Holtz.CQRS.Infraestructure.Persistence
{
    public class ApplicationContext : IApplicationContext
    {
        private static List<Product> _products = new List<Product>();
        public ApplicationContext()
        {
            _products = new List<Product>
            {
                new Product("Pen", "Blue with lid", 0.98),
                new Product("Pen", "Blue without lid", 0.94)
            };
        }

        public IList<Product> Products { get => _products; set => _products = (List<Product>)value; }
    }
}
