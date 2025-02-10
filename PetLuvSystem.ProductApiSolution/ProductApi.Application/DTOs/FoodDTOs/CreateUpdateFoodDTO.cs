namespace ProductApi.Application.DTOs.FoodDTOs
{
    public record CreateUpdateFoodDTO
        (
        string FoodName,
        string FoodDesc,
        string Brand,
        string Ingredient,
        string Origin,
        string AgeRange,
        decimal CountInStock
    );
}
