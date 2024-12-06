using Learning_Web.API.Models.Domain;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.Response
{
    public class WalkResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("walk_image_url")]
        public string? WalkImageUrl { get; set; }
        [JsonPropertyName("length_in_km")]
        public double LengthInKm { get; set; }
        [JsonPropertyName("difficulty")]
        public Guid DifficultyId { get; set; }
        [JsonPropertyName("region")]
        public Guid RegionId { get; set; }
        // Navigation property
        [JsonPropertyName("difficulty")]
        public DifficultyResponse Difficulty { get; set; }
        // Navigation property
        [JsonPropertyName("region")]
        public RegionResponse Region { get; set; }
    }
}
