namespace ServiceApi.Application.DTOs.ServiceComboVariantDTOs
{
    public record CreateUpdateServiceComboVariantDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string? WeightRange,
        decimal ComboPrice,
        bool IsVisible
    );
}
