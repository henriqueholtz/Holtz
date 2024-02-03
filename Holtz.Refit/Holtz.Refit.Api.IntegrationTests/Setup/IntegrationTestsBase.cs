using WireMock.Server;

namespace Holtz.Refit.Api.IntegrationTests.Setup
{
    public class IntegrationTestsBase : IClassFixture<ApiWebApplicationFactory>
    {
        internal HttpClient Client;
        internal ApiWebApplicationFactory Factory;
        internal WireMockServer RandomDataApiServer;
        public IntegrationTestsBase(ApiWebApplicationFactory fixture)
        {
            Factory = fixture;
            Client = Factory.CreateClient();
            RandomDataApiServer = Factory.GetRandomDataApiServer();
        }
    }
}
