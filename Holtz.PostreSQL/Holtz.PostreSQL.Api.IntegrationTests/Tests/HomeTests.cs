using Holtz.PostreSQL.Api.IntegrationTests.Setup;
using System.Net;

namespace Holtz.PostreSQL.Api.IntegrationTests.Tests
{
    public class HomeTests
    {
        [Fact]
        public async Task GET_Home_ShouldBeOk()
        {
            // Arrange
            await using var application = new ApiWebApplicationFactory();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/home");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
