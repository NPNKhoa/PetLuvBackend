using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.ServiceComboVariantDTOs
{
    public record ServiceComboVariantDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string? WeightRange,
        decimal ComboPrice,
        bool IsVisible,
        ServiceCombo? ServiceCombo
    );
}
