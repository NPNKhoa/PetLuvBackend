namespace ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs
{
    public record WalkDogServiceVariantDTO
    (
        Guid ServiceId,
        Guid BreedId,
        string BreedName,
        decimal PricePerPeriod,
        int Period,
        bool IsVisible
    );
}
