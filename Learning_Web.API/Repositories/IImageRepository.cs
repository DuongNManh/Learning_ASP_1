using Learning_Web.API.Models.Domain;

namespace Learning_Web.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> SaveImageAsync(Image image);
    }
}
