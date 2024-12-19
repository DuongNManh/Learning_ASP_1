using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Web.API.Models.DTO;
using AutoMapper;
using Learning_Web.API.CustomActionFilters;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Repositories;
using Learning_Web.API.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Learning_Web.API.Models;

namespace Learning_Web.API.Controllers
{
    // https://localhost:5001/api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IMapper mapper, IWalkRepository walkRepository, ILogger<WalksController> logger)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
            _logger = logger;
        }


        [HttpGet] // GET: https://localhost:5001/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageSize=10&pageNumber=1
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllWalks(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
        {
            var walks = await _walkRepository.GetAllAsync(
                filterOn, filterQuery, sortBy,
                isAscending ?? true, pageNumber, pageSize);

            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<IEnumerable<WalkResponse>>(walks),
                "Walks retrieved successfully");

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetWalk(Guid id)
        {

            var walk = await _walkRepository.GetByIdAsync(id);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<WalkResponse>(walk),
                "Walk retrieved successfully");
            return Ok(response);
        }

        [HttpPost]
        [ValidateModelAttributes]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> CreateWalk([FromBody] WalkDTO walkDTO)
        {
            var walk = _mapper.Map<Walk>(walkDTO);
            var createdWalk = await _walkRepository.CreateAsync(walk);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<WalkResponse>(createdWalk),
                "Walk created successfully",
                201);

            return CreatedAtAction(nameof(GetWalk),
                new { id = createdWalk.Id },
                response);

        }

        [HttpPut]
        //[Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        [ValidateModelAttributes]
        public async Task<IActionResult> UpdateWalk(Guid id, WalkDTO walkDTO)
        {

            var walkUpdated = await _walkRepository.UpdateAsync(id, _mapper.Map<Walk>(walkDTO));
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<WalkResponse>(walkUpdated),
                "Walk updated successfully");
            return Ok(response);
        }

        [HttpDelete]
        //[Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkToDelete = await _walkRepository.DeleteAsync(id);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<WalkResponse>(walkToDelete),
                "Walk deleted successfully");
            return Ok(response);
        }
    }
}
