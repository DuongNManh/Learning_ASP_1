using Learning_Web.API.Data;
using Learning_Web.API.Models.Domain;

namespace Learning_Web.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly WalkDBContext _dbContext;

        // constructor
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            WalkDBContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }


        public async Task<Image> SaveImageAsync(Image image)
        {
            // create local path for image
            var localFilePath =
                Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            // write image to local path
            await using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:5001/Images/{image.FileName}.{image.FileExtension}

            var prefix = _httpContextAccessor.HttpContext.Request;

            var urlFilePath =
                $"{prefix.Scheme}://{prefix.Host}{prefix.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //Add image to database
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }
    }
}
