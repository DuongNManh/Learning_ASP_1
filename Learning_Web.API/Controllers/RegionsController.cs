using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models.DTO;
using Learning_Web.API.Reposotories;
using AutoMapper;

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

        [HttpGet] // GET: https://localhost:5001/api/regions
        public async Task<IActionResult> GetAll()
        {
            // Get all regions
            var regions = await regionRepository.GetAllAsync();

            // Convert each Region to RegionResponse
            var regionResponses = mapper.Map<IEnumerable<RegionResponse>>(regions);

            return Ok(regionResponses);
        }

        [HttpGet] // GET: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        public async Task<IActionResult> Get(Guid id)
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
        public async Task<IActionResult> Post([FromBody] RegionDTO newRegion)
        {
            //Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Convert the RegionDTO to a Region
            var regionToCreate = mapper.Map<Region>(newRegion);

            //Create the region
            var createdRegion = await regionRepository.CreateAsync(regionToCreate);

            //Return the created region
            return CreatedAtAction(nameof(Get), new { id = regionToCreate.Id }, mapper.Map<RegionResponse>(createdRegion));
        }

        [HttpPut] // PUT: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        public async Task<IActionResult> Put(Guid id, [FromBody] RegionDTO updateRegion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var regionToUpdate = await regionRepository.UpdateAsync(id, mapper.Map<Region>(updateRegion));

            if (regionToUpdate == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionResponse>(regionToUpdate));
        }

        [HttpDelete] // PUT: https://localhost:5001/api/regions/1
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        public async Task<IActionResult> Delete(Guid id)
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
