using ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class WalkDogServiceVariantConversion
    {
        public static WalkDogServiceVariant ToEntity(WalkDogServiceVariantDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            BreedId = dto.BreedId,
            PricePerPeriod = dto.PricePerPeriod,
            IsVisible = dto.IsVisible
        };

        public static (WalkDogServiceVariantDTO?, IEnumerable<WalkDogServiceVariantDTO>?) FromEntity(WalkDogServiceVariant? serviceVariant, IEnumerable<WalkDogServiceVariant>? serviceVariants)
        {
            if (serviceVariant is not null && serviceVariants is null)
            {
                var singleServiceVariant = new WalkDogServiceVariantDTO
                (
                    serviceVariant.ServiceId,
                    serviceVariant.BreedId,
                    serviceVariant.PricePerPeriod,
                    serviceVariant.IsVisible
                );
                return (singleServiceVariant, null);
            }

            if (serviceVariants is not null && serviceVariant is null)
            {
                var _serviceVariants = serviceVariants.Select(p => new WalkDogServiceVariantDTO
                (
                    p.ServiceId,
                    p.BreedId,
                    p.PricePerPeriod,
                    p.IsVisible
                )).ToList();

                return (null, _serviceVariants);
            }

            return (null, null);
        }
    }
}
