using Holtz.CQRS.Api.IntegrationTests.Setup;
using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Infraestructure.Persistence;
using Holtz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;

namespace Holtz.CQRS.Api.IntegrationTests
{
    public class ProductsTests : IClassFixture<HoltzCqrsApiFactory>
    {
        private readonly HttpClient _client;
        private readonly HoltzCqrsApiFactory _factory;
        public ProductsTests(HoltzCqrsApiFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Get_Products_Successfully()
        {
            // Arranje
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();

                // Cleaning the database
                await db.Products.ExecuteDeleteAsync();

                Product product1 = new Product("Laptop", "15'", 8759.90);
                Product product2 = new Product("Laptop", "17'", 9659.80);
                db.Products.AddRange([product1, product2]);
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            List<ProductDto>? responseAsProductsList = JsonConvert.DeserializeObject<List<ProductDto>>(await response.Content.ReadAsStringAsync() ?? "");
            responseAsProductsList.Should().HaveCount(2);
            responseAsProductsList.Should().ContainSingle(p => p.Price.Equals(8759.90));
            responseAsProductsList.Should().ContainSingle(p => p.Price.Equals(9659.80));
        }

        [Fact]
        public async Task Get_Product_Successfully()
        {
            // Arranje
            Guid? productId;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();

                // Cleaning the database
                await db.Products.ExecuteDeleteAsync();

                Product product = new Product("Laptop", "Portable", 5721.99);
                productId = product.Id;
                db.Products.Add(product);
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/api/products/{productId}");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            ProductDto? responseAsProduct = JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync() ?? "");
            responseAsProduct.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_NonexistentProduct_NotFound()
        {
            // Arranje
            Guid? productId = Guid.Empty;

            // Act
            var response = await _client.GetAsync($"/api/products/{productId}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}