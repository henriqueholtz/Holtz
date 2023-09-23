using Holtz.Catalog.Microservices.DAL.Entities;
using MongoDB.Driver;

namespace Holtz.Catalog.Microservices.DAL.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
