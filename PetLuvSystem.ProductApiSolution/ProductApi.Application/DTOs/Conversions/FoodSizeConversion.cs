using ProductApi.Application.DTOs.FoodSizeDTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class FoodSizeConversion
    {
        public static FoodSize ToEntity(FoodSizeDTO foodSize) => new()
        {
            SizeId = foodSize.SizeId,
            Size = foodSize.Size,
            Isvisible = foodSize.IsVisible
        };

        public static FoodSize ToEntity(string foodSizeValue) => new()
        {
            SizeId = Guid.Empty,
            Size = foodSizeValue,
            Isvisible = true
        };

        public static (FoodSizeDTO?, IEnumerable<FoodSizeDTO>?) FromEntity(FoodSize entity, IEnumerable<FoodSize> entities)
        {

            if (entity is not null && entities is null)
            {
                return (new FoodSizeDTO(entity.SizeId, entity.Size, entity.Isvisible, null!), null);
            }

            if (entities is not null && entity is null)
            {
                var FoodSizeDTOs = entities.Select(e => new FoodSizeDTO(e.SizeId, e.Size, e.Isvisible, null!)).ToList();
                return (null, FoodSizeDTOs);
            }

            return (null, null);
        }
    }
}
