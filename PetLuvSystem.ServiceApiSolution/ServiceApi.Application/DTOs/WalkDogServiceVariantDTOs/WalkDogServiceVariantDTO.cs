using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs
{
    public record WalkDogServiceVariantDTO
    (
        Guid ServiceId,
        decimal PricePerPeriod,
        Service? Service
    );
}
