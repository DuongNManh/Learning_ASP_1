using Learning_Web.API.Models.Domain;
using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Converters
{
    public static class ModelConverter
    {
        public static RegionResponse Convert(Region region)
        {
            return new RegionResponse
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
        }

        public static DifficultyResponse Convert(Difficulty difficulty)
        {
            return new DifficultyResponse
            {
                Id = difficulty.Id,
                Name = difficulty.Name
            };
        }

        public static WalkResponse Convert(Walk walk)
        {
            return new WalkResponse
            {
                Id = walk.Id,
                Name = walk.Name,
                Description = walk.Description,
                WalkImageUrl = walk.WalkImageUrl,
                LengthInKm = walk.LengthInKm,
            };
        }
    }

}
