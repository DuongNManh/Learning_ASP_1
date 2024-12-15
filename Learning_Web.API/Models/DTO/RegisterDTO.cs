using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.DTO
{
    public class RegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("roles")]
        public string[] Roles { get; set; }
    }
}
