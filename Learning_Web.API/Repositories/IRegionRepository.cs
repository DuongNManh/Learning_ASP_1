using Learning_Web.API.Models;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Web.API.Repositories
{
    public interface IRegionRepository
    {
        Task<PageResponse<Region>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50);
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
