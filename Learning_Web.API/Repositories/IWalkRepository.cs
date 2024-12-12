using Learning_Web.API.Models.Domain;

namespace Learning_Web.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool? isAscending = true);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk> CreateAsync(Walk newWalk);
        Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
