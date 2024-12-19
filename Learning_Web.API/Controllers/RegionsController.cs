using System.Text.Json;
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
using Learning_Web.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Learning_Web.API.Controllers
{
    // https://localhost:5001/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository
            , IMapper mapper, ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet] // GET: https://localhost:5001/api/regions?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageSize=10&pageNumber=1
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllRegions(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
        {
            // Get all regions
            var regions = await _regionRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,
                pageNumber, pageSize);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<IEnumerable<RegionResponse>>(regions),
                "Regions retrieved successfully",
                200
            );
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")] //only if the id is a guid, the request will be performed
        //[Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetRegion(Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<RegionResponse>(region),
                "Region retrieved successfully",
                200
            );
            return Ok(response);
        }

        [HttpPost]
        [ValidateModelAttributes] // Custom Action Filter for Model Validation
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionDTO newRegion)
        {
            var regionToCreate = _mapper.Map<Region>(newRegion);
            var createdRegion = await _regionRepository.CreateAsync(regionToCreate);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<RegionResponse>(createdRegion),
                "Region created successfully",
                201);
            return CreatedAtAction(nameof(GetRegion),
                new { id = createdRegion.Id },
                response);
        }

        [HttpPut]
        //[Authorize(Roles = "writer")]
        [Route("{id:guid}")]
        [ValidateModelAttributes]
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] RegionDTO updateRegion)
        {
            var regionToUpdate = await _regionRepository.UpdateAsync(id, _mapper.Map<Region>(updateRegion));
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<RegionResponse>(regionToUpdate),
                "Region updated successfully");
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var regionToDelete = await _regionRepository.DeleteAsync(id);
            var response = ApiResponseBuilder.BuildResponse(
                _mapper.Map<RegionResponse>(regionToDelete),
                "Region deleted successfully");

            return Ok(response);
        }
    }
}
