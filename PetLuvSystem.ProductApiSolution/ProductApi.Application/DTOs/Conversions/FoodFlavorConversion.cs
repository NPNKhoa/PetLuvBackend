using ProductApi.Application.DTOs.FoodFlavorDTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class FoodFlavorConversion
    {
        public static FoodFlavor ToEntity(FoodFlavorDTO foodFlavor) => new()
        {
            FlavorId = foodFlavor.FlavorId,
            Flavor = foodFlavor.Flavor,
            IsVisible = foodFlavor.IsVisible
        };

        public static FoodFlavor ToEntity(string foodFlavorValue) => new()
        {
            FlavorId = Guid.Empty,
            Flavor = foodFlavorValue,
            IsVisible = true
        };

        public static (FoodFlavorDTO?, IEnumerable<FoodFlavorDTO>?) FromEntity(FoodFlavor entity, IEnumerable<FoodFlavor> entities)
        {

            if (entity is not null && entities is null)
            {
                return (new FoodFlavorDTO(entity.FlavorId, entity.Flavor, entity.IsVisible, null!), null);
            }

            if (entities is not null && entity is null)
            {
                var foodFlavorDTOs = entities.Select(e => new FoodFlavorDTO(e.FlavorId, e.Flavor, e.IsVisible, null!)).ToList();
                return (null, foodFlavorDTOs);
            }

            return (null, null);
        }
    }
}
