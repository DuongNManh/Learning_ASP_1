using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.DTO
{
    public class RegionDTO
    {
        [JsonPropertyName("name")]
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Region Name must be between 3 and 100 characters")]
        public string Name { get; set; }
         
        [JsonPropertyName("code")]
        [Required]
        [Range(1, 999, ErrorMessage = "The Region Code must be in range (1, 999)")]
        public string Code { get; set; }

        [JsonPropertyName("region_image_url")]
        public string? RegionImageUrl { get; set; }
    }
}
