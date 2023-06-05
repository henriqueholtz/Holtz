using Holtz.Domain.Entities;

namespace Holtz.CQRS.Application.Interfaces
{
    public interface IApplicationContext
    {
        public IList<Product> Products { get; set; }
    }
}
