using Learning_Web.API.Models;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Reposotories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
