namespace ServiceApi.Application.DTOs.ServiceComboVariantDTOs
{
    public record ServiceComboVariantDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string BreedName,
        string? WeightRange,
        decimal ComboPrice,
        bool IsVisible
    );
}
