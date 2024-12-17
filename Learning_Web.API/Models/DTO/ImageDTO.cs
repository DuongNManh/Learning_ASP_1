using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.DTO
{
    public class ImageDTO
    {
        [JsonPropertyName("file")]
        public IFormFile File { get; set; }
        [JsonPropertyName("file_name")]
        public string FileName { get; set; }
        [JsonPropertyName("file_description")]
        public string? FileDescription { get; set; }
    }
}
