using ProductApi.Application.DTOs.FoodVariantDTOs;

namespace ProductApi.Application.DTOs.FoodDTOs
{
    public record FoodDTO
    (
        Guid FoodId,
        string FoodName,
        string FoodDesc,
        string Brand,
        string Ingredient,
        string Origin,
        string AgeRange,
        decimal CountInStock,
        bool IsVisible,
        IEnumerable<string> FoodImagePaths,
        IEnumerable<BriefFoodVariantDTO> FoodVariants
    );
}
