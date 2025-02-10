using ProductApi.Application.DTOs.FoodVariantDTOs;

namespace ProductApi.Application.DTOs.FoodSizeDTOs
{
    public record FoodSizeDTO
    (
        Guid SizeId,
        string Size,
        bool IsVisible,
        IEnumerable<FoodVariantDTO> FoodVariants
    );
}
