using System.Text.Json.Serialization;

namespace Holtz.Refit.Domain
{
    public class User
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;
    }
}
