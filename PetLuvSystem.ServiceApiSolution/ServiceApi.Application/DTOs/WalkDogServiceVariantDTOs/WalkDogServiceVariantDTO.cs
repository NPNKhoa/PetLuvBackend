namespace ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs
{
    public record WalkDogServiceVariantDTO
    (
        Guid ServiceId,
        Guid BreedId,
        decimal PricePerPeriod
    );
}
