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
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }


        [HttpGet] // GET: https://localhost:5001/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageSize=10&pageNumber=1
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllWalks(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
        {
            return Ok(_mapper.Map<IEnumerable<WalkResponse>>(await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize)));
        }

        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walk = await _walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkResponse>(walk));
        }

        [HttpPost]
        [ValidateModelAttributes]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> CreateWalk([FromBody] WalkDTO walkDTO)
        {
            // Map the WalkDTO to a Walk and create the walk
            var createdWalk = await _walkRepository.CreateAsync(_mapper.Map<Walk>(walkDTO));

            return CreatedAtAction(nameof(GetWalk), new { id = createdWalk.Id }, _mapper.Map<WalkResponse>(createdWalk));

        }

        [HttpPut]
        //[Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        [ValidateModelAttributes]
        public async Task<IActionResult> UpdateWalk(Guid id, WalkDTO walkDTO)
        {
            var walkUpdated = await _walkRepository.UpdateAsync(id, _mapper.Map<Walk>(walkDTO));

            if (walkUpdated == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkResponse>(walkUpdated));
        }

        [HttpDelete]
        //[Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkToDelete = await _walkRepository.DeleteAsync(id);
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
