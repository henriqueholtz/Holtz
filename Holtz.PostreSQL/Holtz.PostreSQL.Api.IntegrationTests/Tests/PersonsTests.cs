using Holtz.PostreSQL.Api.Context;
using Holtz.PostreSQL.Api.IntegrationTests.Extensions;
using Holtz.PostreSQL.Api.IntegrationTests.Setup;
using Holtz.PostreSQL.Api.Models;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
