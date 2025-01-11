using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.ServiceComboPriceDTOs
{
    public record ServiceComboPriceDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string? WeightRange,
        decimal ComboPrice,
        ServiceCombo? ServiceCombo
    );
}
