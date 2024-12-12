using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models.DTO;
using Learning_Web.API.Repositories;
using AutoMapper;
using Learning_Web.API.CustomActionFilters;

namespace Learning_Web.API.Controllers
{
    // https://localhost:5001/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly WalkDBContext _dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(WalkDBContext walkDBContext, IRegionRepository regionRepository
            ,IMapper mapper)
        {
            this._dbContext = walkDBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet] // GET: https://localhost:5001/api/regions?filterOn=Name&filterQuery=Track
        public async Task<IActionResult> GetAllRegions([FromQuery] string? filterOn,
            [FromQuery] string? filterQuery, [FromQuery] string? sortBy,[FromQuery] bool? isAscending)
        {
            // Get all regions
            var regions = await regionRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

            // Convert each Region to RegionResponse
            var regionResponses = mapper.Map<IEnumerable<RegionResponse>>(regions);

            return Ok(regionResponses);
        }

        [HttpGet] // GET: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        public async Task<IActionResult> GetRegion(Guid id)
        {
            //var region = await _dbContext.Regions.FindAsync(id);

            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            // change the region to a RegionResponse
            return Ok(mapper.Map<RegionResponse>(region));
        }

        [HttpPost] // POST: https://localhost:5001/api/regions
        [ValidateModelAttributes] // Custom Action Filter for Model Validation
        public async Task<IActionResult> CreateRegion([FromBody] RegionDTO newRegion)
        {
            //Convert the RegionDTO to a Region
            var regionToCreate = mapper.Map<Region>(newRegion);

            //Create the region
            var createdRegion = await regionRepository.CreateAsync(regionToCreate);

            //Return the created region
            return CreatedAtAction(nameof(GetRegion), new { id = regionToCreate.Id }, mapper.Map<RegionResponse>(createdRegion));
        }

        [HttpPut] // PUT: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        [ValidateModelAttributes] // Custom Action Filter for Model Validation
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] RegionDTO updateRegion)
        {
            var regionToUpdate = await regionRepository.UpdateAsync(id, mapper.Map<Region>(updateRegion));

            if (regionToUpdate == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionResponse>(regionToUpdate));
        }

        [HttpDelete] // DELETE: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        public async Task<IActionResult> DeleteRegion(Guid id)
        {

            var regionToDelete = await regionRepository.DeleteAsync(id);

            if (regionToDelete == null)
            {
                return NotFound();
            }

            var response = new
            {
                data = new
                {
                    type = "regions",
                    id = regionToDelete.Id.ToString(),
                    attributes = new
                    {
                        message = "Region deleted successfully."
                    }
                }
            };

            return Ok(response);
        }
    }
}
