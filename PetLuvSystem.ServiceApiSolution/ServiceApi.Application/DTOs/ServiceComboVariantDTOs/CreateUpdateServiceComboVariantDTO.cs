﻿namespace ServiceApi.Application.DTOs.ServiceComboVariantDTOs
{
    public record CreateUpdateServiceComboVariantDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string? WeightRange,
        decimal ComboPrice,
        int EstimateTime,
        bool IsVisible
    );
}
