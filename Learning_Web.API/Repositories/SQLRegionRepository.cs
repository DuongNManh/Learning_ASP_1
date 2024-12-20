using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Learning_Web.API.Exceptions;
using Learning_Web.API.Models;

namespace Learning_Web.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly WalkDBContext _dbContext;
        private readonly ILogger<SQLRegionRepository> _logger;

        public SQLRegionRepository(WalkDBContext dbContext, ILogger<SQLRegionRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PageResponse<Region>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 50)
        {
            try
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
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var skipAmount = (pageNumber - 1) * pageSize;
                var items = await query.Skip(skipAmount).Take(pageSize).ToListAsync();

                return new PageResponse<Region>
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
                _logger.LogError(ex, "Error occurred while retrieving all regions");
                throw new ApiException("Failed to retrieve regions", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            try
            {
                var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                if (region == null)
                {
                    throw new NotFoundException($"Region with ID {id} was not found");
                }
                return region;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving region by ID");
                throw new ApiException("Failed to retrieve region", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Region> CreateAsync(Region region)
        {
            try
            {
                await _dbContext.Regions.AddAsync(region);
                await _dbContext.SaveChangesAsync();
                return region;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating region");
                throw new ApiException("Failed to create region", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            try
            {
                var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                if (existingRegion == null)
                {
                    throw new NotFoundException($"Region with ID {id} was not found");
                }

                existingRegion.Name = region.Name;
                existingRegion.Code = region.Code;
                existingRegion.RegionImageUrl = region.RegionImageUrl;

                await _dbContext.SaveChangesAsync();
                return existingRegion;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating region");
                throw new ApiException("Failed to update region", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            try
            {
                var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                if (region == null)
                {
                    throw new NotFoundException($"Region with ID {id} was not found");
                }

                _dbContext.Regions.Remove(region);
                await _dbContext.SaveChangesAsync();
                return region;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting region");
                throw new ApiException("Failed to delete region", HttpStatusCode.InternalServerError);
            }
        }
    }
}
