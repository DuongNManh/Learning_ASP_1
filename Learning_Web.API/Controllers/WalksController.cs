using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Web.API.Models.DTO;
using AutoMapper;
using Learning_Web.API.CustomActionFilters;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Repositories;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Authorization;

namespace Learning_Web.API.Controllers
{
    // https://localhost:5001/api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        [HttpGet] // GET: https://localhost:5001/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageSize=10&pageNumber=1
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllWalks(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
        {
            return Ok(mapper.Map<IEnumerable<WalkResponse>>(await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize)));
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walk = await walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkResponse>(walk));
        }

        [HttpPost]
        [ValidateModelAttributes]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> CreateWalk([FromBody] WalkDTO walkDTO)
        {
            // Map the WalkDTO to a Walk and create the walk
            var createdWalk = await walkRepository.CreateAsync(mapper.Map<Walk>(walkDTO));

            return CreatedAtAction(nameof(GetWalk), new { id = createdWalk.Id }, mapper.Map<WalkResponse>(createdWalk));

        }

        [HttpPut]
        [Authorize]
        [Route("{id:guid}")]
        [ValidateModelAttributes]
        public async Task<IActionResult> UpdateWalk(Guid id, WalkDTO walkDTO)
        {
            var walkUpdated = await walkRepository.UpdateAsync(id, mapper.Map<Walk>(walkDTO));

            if (walkUpdated == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkResponse>(walkUpdated));
        }

        [HttpDelete]
        [Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkToDelete = await walkRepository.DeleteAsync(id);
            if (walkToDelete == null)
            {
                return NotFound();
            }
            var response = new
            {
                data = new
                {
                    type = "walks",
                    id = walkToDelete.Id.ToString(),
                    attributes = new
                    {
                        message = "Walk deleted successfully."
                    }
                }
            };

            return Ok(response);
        }
    }
}
