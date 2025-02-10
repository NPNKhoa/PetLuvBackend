using ProductApi.Application.DTOs.FoodDTOs;
using ProductApi.Application.DTOs.FoodVariantDTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class FoodConversion
    {
        public static Food ToEntity(FoodDTO dto) => new()
        {
            FoodId = dto.FoodId,
            FoodName = dto.FoodName,
            FoodDesc = dto.FoodDesc,
            Brand = dto.Brand,
            Ingredient = dto.Ingredient,
            Origin = dto.Origin,
            AgeRange = dto.AgeRange,
            CountInStock = dto.CountInStock,
            IsVisible = dto.IsVisible
        };

        public static Food ToEntity(CreateUpdateFoodDTO dto) => new()
        {
            FoodId = Guid.Empty,
            FoodName = dto.FoodName,
            FoodDesc = dto.FoodDesc,
            Brand = dto.Brand,
            Ingredient = dto.Ingredient,
            Origin = dto.Origin,
            AgeRange = dto.AgeRange,
            CountInStock = dto.CountInStock,
            IsVisible = true
        };

        public static (FoodDTO?, IEnumerable<FoodDTO>?) FromEntity(Food entity, IEnumerable<Food> entities)
        {

            if (entity is not null && entities is null)
            {
                return (new FoodDTO(
                    entity.FoodId,
                    entity.FoodName,
                    entity.FoodDesc,
                    entity.Brand,
                    entity.Ingredient,
                    entity.Origin,
                    entity.AgeRange,
                    entity.CountInStock,
                    entity.IsVisible,
                    entity.FoodImages?.Select(fi => fi.FoodImagePath) ?? null!,
                    entity.FoodVariants?.Select(fv => new BriefFoodVariantDTO(
                        entity.FoodId,
                        fv.Flavor?.Flavor ?? string.Empty,
                        fv.Size?.Size ?? string.Empty,
                        fv.Price,
                        fv.Isvisible
                    )) ?? null!
                ), null);
            }

            if (entity is null && entities is not null)
            {
                return (null, entities.Select(e => new FoodDTO(
                    e.FoodId,
                    e.FoodName,
                    e.FoodDesc,
                    e.Brand,
                    e.Ingredient,
                    e.Origin,
                    e.AgeRange,
                    e.CountInStock,
                    e.IsVisible,
                    null!,
                    null!
                )).ToList());
            }

            return (null, null);
        }
    }
}
