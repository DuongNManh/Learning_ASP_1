using AutoMapper;
using Learning_Web.API.Data;
using Learning_Web.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Learning_Web.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly WalkDBContext _dBContext;
        private readonly IMapper _mapper;

        public SQLWalkRepository(WalkDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<Walk> CreateAsync(Walk newWalk)
        {
            // Add the new walk to the database
            await _dBContext.Walks.AddAsync(newWalk);
            await _dBContext.SaveChangesAsync();
            return newWalk;

        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await _dBContext.Walks.FindAsync(id);
            if (walk == null)
            {
                return null;
            }

            _dBContext.Walks.Remove(walk);
            await _dBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50)
        {
            var query = _dBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // filter the query

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "name" => query.Where(x => x.Name.Contains(filterQuery)),
                    "length" => double.TryParse(filterQuery, out double length)
                        ? query.Where(x => x.LengthInKm == length)
                        : query,
                    "difficulty" => query.Where(x => x.Difficulty.Name.Contains(filterQuery)),
                    "region" => query.Where(x => x.Region.Name.Contains(filterQuery)),
                    _ => query
                };
            }

            // sort the query
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending.GetValueOrDefault()
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending.GetValueOrDefault()
                        ? query.OrderBy(x => x.LengthInKm)
                        : query.OrderByDescending(x => x.LengthInKm);
                }
            }

            // paginate the query
            var skipAmount = pageSize * (pageNumber - 1); // how many records to skip

            // end of the query
            return await query.Skip(skipAmount).Take(pageSize).ToListAsync(); // now the request is ready to be sent to the database
        }


        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk)
        {
            var walk = await _dBContext.Walks.FindAsync(id);
            if (walk == null)
            {
                return null;
            }

            walk.Name = updatedWalk.Name;
            walk.Description = updatedWalk.Description;
            walk.WalkImageUrl = updatedWalk.WalkImageUrl;
            walk.LengthInKm = updatedWalk.LengthInKm;
            walk.DifficultyId = updatedWalk.DifficultyId;
            walk.RegionId = updatedWalk.RegionId;

            // save the changes to the database
            await _dBContext.SaveChangesAsync();
            return walk;
        }
    }
}
