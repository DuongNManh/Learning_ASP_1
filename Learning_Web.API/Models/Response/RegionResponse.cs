using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.Response
{
    public class RegionResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("region_image_url")]
        public string? RegionImageUrl { get; set; }
    }
}
