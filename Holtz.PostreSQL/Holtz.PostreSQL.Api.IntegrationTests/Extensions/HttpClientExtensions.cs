using Newtonsoft.Json;

namespace Holtz.PostreSQL.Api.IntegrationTests.Extensions {
    public static class Extensions
    {
        public static async Task<T?> GetAndDeserializeAsync<T>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }

}