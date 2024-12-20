using AutoMapper;
using Learning_Web.API.Data;
using Learning_Web.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Learning_Web.API.Exceptions;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models;

namespace Learning_Web.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly WalkDBContext _dBContext;
        private readonly ILogger<SQLWalkRepository> _logger;

        public SQLWalkRepository(WalkDBContext dBContext, ILogger<SQLWalkRepository> logger)
        {
            _dBContext = dBContext;
            _logger = logger;
        }

        public async Task<Walk> CreateAsync(Walk newWalk)
        {
            try
            {
                await _dBContext.Walks.AddAsync(newWalk);
                await _dBContext.SaveChangesAsync();
                return newWalk;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new walk");
                throw new ApiException("Failed to create walk", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PageResponse<Walk>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50)
        {
            try
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

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var skipAmount = (pageNumber - 1) * pageSize;
                var items = await query.Skip(skipAmount).Take(pageSize).ToListAsync();

                return new PageResponse<Walk>
                {
                    Items = items,
                    Meta = new PaginationMeta
                    {
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalItems = totalItems,
                        TotalPages = totalPages
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all walks");
                throw new Exception("Error occurred while retrieving all walks", ex);
            }
        }


        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            try
            {
                var walk = await _dBContext.Walks
                    .Include(x => x.Difficulty)
                    .Include(x => x.Region)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (walk == null)
                {
                    throw new NotFoundException($"Walk with ID {id} was not found");
                }

                return walk;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving walk");
                throw new ApiException("Failed to retrieve walk", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the walk");
                throw new Exception("Error occurred while updating the walk", ex);
            }
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the walk");
                throw new Exception("Error occurred while deleting the walk", ex);
            }
        }
    }
}
