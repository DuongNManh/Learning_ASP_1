using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}