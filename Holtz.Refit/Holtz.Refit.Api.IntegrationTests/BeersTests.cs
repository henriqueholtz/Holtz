using Holtz.Refit.Api.IntegrationTests.Extensions;
using Holtz.Refit.Api.IntegrationTests.Setup;
using Holtz.Refit.Api.IntegrationTests.WireMockServers;
using Holtz.Refit.Domain;
using System.Net;
using System.Net.Mime;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Holtz.Refit.Api.IntegrationTests
{
    public class BeersTests : IDisposable
    {
        private WireMockServer server = WireMockServer.Start(RandomDataApiWireMockServer.DEFAULT_PORT);

        public void Dispose()
        {
            server.Stop();
        }

        [Fact]
        public async Task Get_Beers_Successfully()
        {
            // Arrange
            server.Given(
                    Request.Create().WithPath("/api/v2/beers").UsingGet()
                ).RespondWith(
                    Response.Create().WithStatusCode(200).WithHeader("Content-Type", MediaTypeNames.Application.Json)
                    .WithBody("[{\"id\":8653,\"uid\":\"88e0c38c-cb97-43c7-9c22-e5ff9f6216b9\",\"brand\":\"Lowenbrau\",\"name\":\"Two Hearted Ale\",\"style\":\"English Brown Ale\",\"hop\":\"Tettnang\",\"yeast\":\"3278 - Belgian Lambic Blend\",\"malts\":\"Caramel\",\"ibu\":\"42 IBU\",\"alcohol\":\"8.1%\",\"blg\":\"12.7°Blg\"},{\"id\":5830,\"uid\":\"d47caffe-aaca-4271-ab56-fd86dd362211\",\"brand\":\"Birra Moretti\",\"name\":\"Trois Pistoles\",\"style\":\"Amber Hybrid Beer\",\"hop\":\"Brewer’s Gold\",\"yeast\":\"1007 - German Ale\",\"malts\":\"Vienna\",\"ibu\":\"53 IBU\",\"alcohol\":\"10.0%\",\"blg\":\"6.8°Blg\"},{\"id\":5540,\"uid\":\"b0884e27-36db-4fc9-8072-dbb6baa78ef0\",\"brand\":\"Harp\",\"name\":\"Bell’s Expedition\",\"style\":\"India Pale Ale\",\"hop\":\"Brewer’s Gold\",\"yeast\":\"3333 - German Wheat\",\"malts\":\"Vienna\",\"ibu\":\"81 IBU\",\"alcohol\":\"9.7%\",\"blg\":\"15.7°Blg\"},{\"id\":354,\"uid\":\"7ba06a39-2c39-445a-8963-8ea0b32d50ba\",\"brand\":\"Delirium\",\"name\":\"Schneider Aventinus\",\"style\":\"Light Hybrid Beer\",\"hop\":\"Warrior\",\"yeast\":\"3724 - Belgian Saison\",\"malts\":\"Carapils\",\"ibu\":\"75 IBU\",\"alcohol\":\"8.3%\",\"blg\":\"5.9°Blg\"},{\"id\":3522,\"uid\":\"939bc3c1-8615-4b94-9047-e18cd7cb7e7a\",\"brand\":\"Heineken\",\"name\":\"Founders Breakfast Stout\",\"style\":\"Merican Ale\",\"hop\":\"Horizon\",\"yeast\":\"1099 - Whitbread Ale\",\"malts\":\"Caramel\",\"ibu\":\"43 IBU\",\"alcohol\":\"6.3%\",\"blg\":\"11.8°Blg\"}]")
                );
            await using var application = new ApiWebApplicationFactory();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAndDeserializeAsync<List<Beer>>("/api/beers");
            
            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(5);
        }

        [Fact]
        public async Task Get_Beers_Error()
        {
            // Arrange
            server.Given(
                    Request.Create().WithPath("/api/v2/beers").UsingGet()
                ).RespondWith(
                    Response.Create().WithStatusCode(500).WithHeader("Content-Type", MediaTypeNames.Application.Json)
                    .WithBody("{\"message\": \"Unexpected error\"}")
                );
            await using var application = new ApiWebApplicationFactory();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/beers");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}