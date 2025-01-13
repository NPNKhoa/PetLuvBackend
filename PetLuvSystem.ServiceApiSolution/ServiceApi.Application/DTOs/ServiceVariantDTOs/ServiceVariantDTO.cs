namespace ServiceApi.Application.DTOs.ServiceVariantDTOs
{
    public record ServiceVariantDTO
    (
        Guid ServiceId,
        Guid BreedId,
        string PetWeightRange,
        decimal Price
    );
}
