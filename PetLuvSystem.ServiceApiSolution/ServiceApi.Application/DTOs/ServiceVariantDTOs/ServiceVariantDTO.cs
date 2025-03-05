namespace ServiceApi.Application.DTOs.ServiceVariantDTOs
{
    public record ServiceVariantDTO
    (
        Guid ServiceId,
        Guid BreedId,
        string BreedName,
        string PetWeightRange,
        decimal Price,
        int EstimateTime,
        bool IsVisible
    );
}
