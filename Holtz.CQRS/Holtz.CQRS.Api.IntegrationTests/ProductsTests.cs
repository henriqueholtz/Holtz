using Holtz.CQRS.Api.IntegrationTests.Setup;
using Holtz.CQRS.Application.DTOs.Products;
using Holtz.CQRS.Infraestructure.Persistence;
using Holtz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
    }
}