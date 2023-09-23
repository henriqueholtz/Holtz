using Holtz.Catalog.Microservices.DAL.Entities;
using MongoDB.Driver;

namespace Holtz.Catalog.Microservices.DAL.CatalogContext
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> mongoCollection)
        {
            bool existProduct = mongoCollection.Find(p => true).Any();
            if (!existProduct)
                mongoCollection.InsertManyAsync(GetProducts());
        }

        private static IEnumerable<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = "1",
                    Name = "Product 1",
                    Description = "Desc 1",
                    Category = "Category 1",
                    Image = "image1.png",
                    Price = 10.10m
                },
                new Product
                {
                    Id = "2",
                    Name = "Product 2",
                    Description = "Desc 2",
                    Category = "Category 2",
                    Image = "image2.png",
                    Price = 20.20m
                },
                new Product
                {
                    Id = "3",
                    Name = "Product 3",
                    Description = "Desc 3",
                    Category = "Category 3",
                    Image = "image3.png",
                    Price = 30.30m
                },
                new Product
                {
                    Id = "4",
                    Name = "Product 4",
                    Description = "Desc 4",
                    Category = "Category 4",
                    Image = "image4.png",
                    Price = 40.40m
                },
                new Product
                {
                    Id = "5",
                    Name = "Product 5",
                    Description = "Desc 5",
                    Category = "Category 5",
                    Image = "image5.png",
                    Price = 50.50m
                },
            };
        }
    }
}
