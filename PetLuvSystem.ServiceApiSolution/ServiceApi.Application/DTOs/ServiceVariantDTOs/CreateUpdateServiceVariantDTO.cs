namespace ServiceApi.Application.DTOs.ServiceVariantDTOs
{
    public record CreateUpdateServiceVariantDTO
    (
        Guid ServiceId,
        Guid BreedId,
        string PetWeightRange,
        decimal Price,
        bool IsVisible
    );
}
