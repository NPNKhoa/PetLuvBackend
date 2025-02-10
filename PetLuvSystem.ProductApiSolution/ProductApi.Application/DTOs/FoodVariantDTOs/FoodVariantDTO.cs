namespace ProductApi.Application.DTOs.FoodVariantDTOs
{
    public record FoodVariantDTO
    (
        Guid FoodId,
        Guid FlavorId,
        Guid SizeId,
        decimal Price,
        string Flavor,
        string Size,
        bool IsVisible
    );
}
