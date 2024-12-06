using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.Response
{
    public class DifficultyResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
