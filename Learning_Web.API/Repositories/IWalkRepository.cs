using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Repositories
{
    public interface IWalkRepository
    {
        Task<PageResponse<Walk>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk> CreateAsync(Walk newWalk);
        Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
