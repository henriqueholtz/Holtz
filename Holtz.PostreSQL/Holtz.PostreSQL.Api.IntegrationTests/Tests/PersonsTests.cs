using Holtz.PostreSQL.Api.IntegrationTests.Setup;
using System.Net;

namespace Holtz.PostreSQL.Api.IntegrationTests.Tests
{
    public class PersonsTests : IntegrationTest
    {
        public PersonsTests(ApiWebApplicationFactory fixture) : base(fixture) { }
        [Fact]
        public async Task GET_Persons_ShouldBeOk()
        {
            // Act
            var response = await _client.GetAsync("/api/persons");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
