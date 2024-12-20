using System.Text.Json.Serialization;

namespace Learning_Web.API.Models.Response
{
    public class PageResponse<T>
    {
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; }

        [JsonPropertyName("meta")]
        public PaginationMeta Meta { get; set; }
    }
}
