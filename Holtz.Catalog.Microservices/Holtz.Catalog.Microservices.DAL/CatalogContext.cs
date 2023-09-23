using Holtz.Catalog.Microservices.DAL.Entities;
using Holtz.Catalog.Microservices.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Holtz.Catalog.Microservices.DAL
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDb:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("MongoDb:DatabaseName").Value);
            Products = database.GetCollection<Product>(configuration.GetSection("MongoDb:CollectionName").Value);

            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; private set; }
    }
}
