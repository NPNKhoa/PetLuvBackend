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
            Price = dto.Price
        };

        public static ServiceVariant ToEntity(CreateUpdateServiceVariantDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            BreedId = dto.BreedId,
            PetWeightRange = dto.PetWeightRange,
            Price = dto.Price
        };

        public static (ServiceVariantDTO?, IEnumerable<ServiceVariantDTO>?) FromEntity(ServiceVariant? serviceVariant, IEnumerable<ServiceVariant>? serviceVariants)
        {
            if (serviceVariant is not null && serviceVariants is null)
            {
                var singleServiceVariant = new ServiceVariantDTO
                (
                    serviceVariant.ServiceId,
                    serviceVariant.BreedId,
                    serviceVariant.PetWeightRange!,
                    serviceVariant.Price
                );
                return (singleServiceVariant, null);
            }

            if (serviceVariants is not null && serviceVariant is null)
            {
                var _serviceVariants = serviceVariants.Select(p => new ServiceVariantDTO
                (
                    p.ServiceId,
                    p.BreedId,
                    p.PetWeightRange!,
                    p.Price
                )).ToList();

                return (null, _serviceVariants);
            }

            return (null, null);
        }
    }
}
