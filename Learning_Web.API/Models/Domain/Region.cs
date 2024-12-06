namespace Learning_Web.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; } // Unique identifier
        public string Name { get; set; } // Region name
        public string Code { get; set; } // Region code
        public string? RegionImageUrl { get; set; } // Region image URL
    }
}
