namespace ProductApi.Application.DTOs.FoodVariantDTOs
{
    public record BriefFoodVariantDTO
    (
        Guid FoodId,
        string Flavor,
        string Size,
        decimal Price,
        bool IsVisible
    );
}
