using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Web.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly WalkDBContext _dbContext;

        public SQLRegionRepository(WalkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50)
        {
            var query = _dbContext.Regions.AsQueryable();

            // filter the query
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.Code.Contains(filterQuery));
                }
            }

            // sort the query
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending == true ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending == true ? query.OrderBy(x => x.Code) : query.OrderByDescending(x => x.Code);
                }
            }

            // paginate the query
            var skipAmount = (pageNumber - 1) * pageSize;

            return await query.Skip(skipAmount).Take(pageSize).ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionToUpdate = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            
            if (regionToUpdate == null)
            {
                return null;
            }

            regionToUpdate.Name = region.Name;
            regionToUpdate.Code = region.Code;
            regionToUpdate.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return regionToUpdate;
        }
    }
}
