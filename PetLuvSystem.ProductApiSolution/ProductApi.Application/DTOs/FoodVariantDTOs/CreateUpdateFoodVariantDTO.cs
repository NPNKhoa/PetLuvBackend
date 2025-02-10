namespace ProductApi.Application.DTOs.FoodVariantDTOs
{
    public record CreateUpdateFoodVariantDTO
    (
        Guid FoodId,
        Guid FlavorId,
        Guid SizeId,
        decimal Price
    );
}
