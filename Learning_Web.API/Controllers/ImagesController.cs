using AutoMapper;
using Learning_Web.API.CustomActionFilters;
using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.DTO;
using Learning_Web.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        //constructor
        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        //POST: https://localhost:5001/api/images/upload
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageDTO imageDTO)
        {
            ValidateFileUpload(imageDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = _mapper.Map<Image>(imageDTO);

            // use repository to save image to database

            var savedImage = await _imageRepository.SaveImageAsync(image);

            return Ok(savedImage);
        }



        private void ValidateFileUpload(ImageDTO imageDTO)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(Path.GetExtension(imageDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "Invalid file extension. Only .jpg, .jpeg, .png, .gif are allowed.");
            }
            if (imageDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "The file size must be less than 10MB.");
            }
        }
    }
}
