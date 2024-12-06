using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Data;
using Microsoft.EntityFrameworkCore;
using Learning_Web.API.Converters;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models.DTO;

namespace Learning_Web.API.Controllers
{
    // https://localhost:5001/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly WalkDBContext _dbContext;

        public RegionsController(WalkDBContext walkDBContext)
        {
            this._dbContext = walkDBContext;
        }

        [HttpGet] // GET: https://localhost:5001/api/regions
        public async Task<IActionResult> GetAll()
        {
            var regions = await _dbContext.Regions.ToListAsync();

            if (regions == null)
            {
                return NotFound();
            }

            // change the list of regions to a list of RegionResponse
            var regionResponses = regions.Select(x => ModelConverter.Convert(x)).ToList();

            return Ok(regionResponses);
        }

        [HttpGet] // GET: https://localhost:5001/api/regions/1
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //var region = await _dbContext.Regions.FindAsync(id);

            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            // change the region to a RegionResponse
            return Ok(ModelConverter.Convert(region));
        }

        [HttpPost] // POST: https://localhost:5001/api/regions
        public async Task<IActionResult> Post([FromBody] RegionDTO region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Convert the RegionDTO to a Region
            var regionToCreate = new Region
            {
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };

            _dbContext.Regions.Add(regionToCreate);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = regionToCreate.Id }, ModelConverter.Convert(regionToCreate));
        }
    }
}
