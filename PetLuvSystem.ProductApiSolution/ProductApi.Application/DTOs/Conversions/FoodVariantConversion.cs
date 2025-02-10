using ProductApi.Application.DTOs.FoodVariantDTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class FoodVariantConversion
    {
        public static FoodVariant ToEntity(FoodVariantDTO foodVariant) => new()
        {
            FoodId = foodVariant.FoodId,
            FlavorId = foodVariant.FlavorId,
            SizeId = foodVariant.SizeId,
            Price = foodVariant.Price,
            Isvisible = foodVariant.IsVisible
        };

        public static FoodVariant ToEntity(CreateUpdateFoodVariantDTO foodVariant) => new()
        {
            FoodId = foodVariant.FoodId,
            FlavorId = foodVariant.FlavorId,
            SizeId = foodVariant.SizeId,
            Price = foodVariant.Price,
            Isvisible = true
        };

        public static (FoodVariantDTO?, IEnumerable<FoodVariantDTO>?) FromEntity(FoodVariant entity, IEnumerable<FoodVariant> entities)
        {

            if (entity is not null && entities is null)
            {
                return (new FoodVariantDTO(
                    entity.FoodId,
                    entity.FlavorId,
                    entity.SizeId,
                    entity.Price,
                    entity.Flavor.Flavor,
                    entity.Size.Size,
                    entity.Isvisible
                ), null);
            }

            if (entity is null && entities is not null)
            {
                return (null, entities.Select(entity => new FoodVariantDTO(
                    entity.FoodId,
                    entity.FlavorId,
                    entity.SizeId,
                    entity.Price,
                    entity.Flavor.Flavor,
                    entity.Size.Size,
                    entity.Isvisible
                )).ToList());
            }

            return (null, null);
        }
    }
}
