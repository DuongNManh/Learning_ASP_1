using Learning_Web.API.Models;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Web.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending);
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
