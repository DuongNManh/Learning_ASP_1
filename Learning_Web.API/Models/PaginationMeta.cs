﻿using System.Text.Json.Serialization;

namespace Learning_Web.API.Models
{
    public class PaginationMeta
    {
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_items")]
        public long TotalItems { get; set; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
    }
}