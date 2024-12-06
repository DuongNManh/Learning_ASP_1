using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.DTO
{
    public class RegionDTO
    {
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        [Required]
        [Range(1, 999)]
        public string Code { get; set; }

        [JsonPropertyName("region_image_url")]
        public string? RegionImageUrl { get; set; }
    }
}
