using ServiceApi.Application.DTOs.ServiceVariantDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceVariantConversion
    {
        public static ServiceVariant ToEntity(ServiceVariantDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            BreedId = dto.BreedId,
            PetWeightRange = dto.PetWeightRange,
            Price = dto.Price,
            EstimateTime = dto.EstimateTime,
            IsVisible = dto.IsVisible
        };

        public static ServiceVariant ToEntity(CreateUpdateServiceVariantDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            BreedId = dto.BreedId,
            PetWeightRange = dto.PetWeightRange,
            Price = dto.Price,
            EstimateTime = dto.EstimateTime,
            IsVisible = dto.IsVisible
        };

        public static (ServiceVariantDTO?, IEnumerable<ServiceVariantDTO>?) FromEntity(ServiceVariant? serviceVariant, IEnumerable<ServiceVariant>? serviceVariants, Dictionary<Guid, string>? breedMapping = null)
        {
            if (serviceVariant is not null && serviceVariants is null)
            {
                var singleServiceVariant = new ServiceVariantDTO
                (
                    serviceVariant.ServiceId,
                    serviceVariant.BreedId,
                    (breedMapping != null && breedMapping.TryGetValue(serviceVariant.BreedId, out var name)) ? name : string.Empty,
                    serviceVariant.PetWeightRange!,
                    serviceVariant.Price,
                    serviceVariant.EstimateTime,
                    serviceVariant.IsVisible
                );
                return (singleServiceVariant, null);
            }

            if (serviceVariants is not null && serviceVariant is null)
            {
                var _serviceVariants = serviceVariants.Select(p => new ServiceVariantDTO
                (
                    p.ServiceId,
                    p.BreedId,
                    (breedMapping != null && breedMapping.TryGetValue(p.BreedId, out var name)) ? name : string.Empty,
                    p.PetWeightRange!,
                    p.Price,
                    p.EstimateTime,
                    p.IsVisible
                )).ToList();

                return (null, _serviceVariants);
            }

            return (null, null);
        }
    }
}
