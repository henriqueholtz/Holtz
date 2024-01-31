namespace Holtz.PostreSQL.Api.IntegrationTests.Setup
{
    // https://timdeschryver.dev/blog/how-to-test-your-csharp-web-api#a-custom-and-reusable-xunit-fixture
    public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        public IntegrationTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }
    }
}
