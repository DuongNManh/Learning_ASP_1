using Learning_Web.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.DTO
{
    public class WalkDTO
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("walk_image_url")]
        public string? WalkImageUrl { get; set; }
        [Range(0.1, double.MaxValue, ErrorMessage = "Length must be greater than 0")]
        [Required]
        [JsonPropertyName("length_in_km")]
        public double LengthInKm { get; set; }
        [Required]
        [JsonPropertyName("difficulty_id")]
        public Guid DifficultyId { get; set; }
        [Required]
        [JsonPropertyName("region_id")]
        public Guid RegionId { get; set; }
    }
}
