using ProductApi.Application.DTOs.FoodVariantDTOs;

namespace ProductApi.Application.DTOs.FoodFlavorDTOs
{
    public record FoodFlavorDTO
    (
        Guid FlavorId,
        string Flavor,
        bool IsVisible,
        IEnumerable<FoodVariantDTO> FoodVariants
    );
}
