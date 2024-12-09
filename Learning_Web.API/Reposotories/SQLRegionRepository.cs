using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Reposotories
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

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
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
