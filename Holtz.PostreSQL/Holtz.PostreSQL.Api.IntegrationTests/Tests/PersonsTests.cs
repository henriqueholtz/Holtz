using Holtz.PostreSQL.Api.Context;
using Holtz.PostreSQL.Api.IntegrationTests.Extensions;
using Holtz.PostreSQL.Api.IntegrationTests.Setup;
using Holtz.PostreSQL.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace Holtz.PostreSQL.Api.IntegrationTests.Tests
{
    public class PersonsTests : IntegrationTest
    {
        public PersonsTests(ApiWebApplicationFactory fixture) : base(fixture) { }

        [Fact]
        public async Task GET_2Persons_ShouldBeOk()
        {
            // Arranje
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<HoltzPostgreSqlContext>();

                Person person1 = new Person { FirstName = "Person A", LastName = "Abc" };
                Person person2 = new Person { FirstName = "Person B", LastName = "Bcd" };
                db.People.AddRange(new List<Person> { person1, person2 });
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAndDeserializeAsync<List<Person>>("/api/persons");

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task POST_AddSinglePerson_ShouldBeOk()
        {
            // Arranje
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<HoltzPostgreSqlContext>();
                
                // Cleaning the database
                await db.People.ExecuteDeleteAsync();
                await db.SaveChangesAsync();
            }
            Person person = new Person { FirstName = "Testing Name", LastName = "Included" };
            StringContent stringContent =  new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/persons", stringContent);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<HoltzPostgreSqlContext>();

                // Cleaning the database
                Person? personDb = await db.People.FirstOrDefaultAsync(p => p.FirstName.Equals("Testing Name"));
                personDb.Should().NotBeNull();
                personDb?.FirstName.Should().Be("Testing Name");
                personDb?.LastName.Should().Be("Included");
            }
        }

        [Fact]
        public async Task GET_SinglePerson_ShouldBeOk()
        {
            // Arranje
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<HoltzPostgreSqlContext>();

                // Cleaning the database
                await db.People.ExecuteDeleteAsync();

                Person person = new Person { Id = 555, FirstName = "Jhon", LastName = "Doe" };
                db.People.Add(person);
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAndDeserializeAsync<Person>("/api/persons/555");

            // Assert
            response.Should().NotBeNull();
            response?.Id.Should().Be(555);
            response?.FirstName.Should().Be("Jhon");
            response?.LastName.Should().Be("Doe");
        }
    }
}
