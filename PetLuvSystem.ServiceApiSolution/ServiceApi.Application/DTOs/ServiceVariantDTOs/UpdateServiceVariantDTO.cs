namespace ServiceApi.Application.DTOs.ServiceVariantDTOs
{
    public record UpdateServiceVariantDTO
    (
        Guid BreedId,
        string PetWeightRange,
        decimal Price,
        int EstimateTime,
        bool IsVisible
    );
}
