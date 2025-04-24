using ServiceApi.Application.DTOs.ServiceComboVariantDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceComboVariantConversion
    {
        public static ServiceComboVariant ToEntity(ServiceComboVariantDTO dto) => new()
        {
            ServiceComboId = dto.ServiceComboId,
            BreedId = dto.BreedId,
            WeightRange = dto.WeightRange,
            ComboPrice = dto.ComboPrice,
            EstimateTime = dto.EstimateTime,
            IsVisible = dto.IsVisible,
        };

        public static ServiceComboVariant ToEntity(CreateUpdateServiceComboVariantDTO dto) => new()
        {
            ServiceComboId = dto.ServiceComboId,
            BreedId = dto.BreedId,
            WeightRange = dto.WeightRange,
            ComboPrice = dto.ComboPrice,
            EstimateTime = dto.EstimateTime,
            IsVisible = dto.IsVisible,
            ServiceCombo = null
        };

        public static (ServiceComboVariantDTO?, IEnumerable<ServiceComboVariantDTO>?) FromEntity(ServiceComboVariant? serviceComboVariant, IEnumerable<ServiceComboVariant>? serviceComboVariants, Dictionary<Guid, string>? breedMapping = null)
        {
            if (serviceComboVariant is not null && serviceComboVariants is null)
            {
                var singleServiceComboVariant = new ServiceComboVariantDTO
                (
                    serviceComboVariant.ServiceComboId,
                    serviceComboVariant.BreedId,
                    (breedMapping != null && breedMapping.TryGetValue(serviceComboVariant.BreedId, out var name)) ? name : string.Empty,
                    serviceComboVariant.WeightRange,
                    serviceComboVariant.ComboPrice,
                    serviceComboVariant.EstimateTime,
                    serviceComboVariant.IsVisible
                );
                return (singleServiceComboVariant, null);
            }

            if (serviceComboVariants is not null && serviceComboVariant is null)
            {
                var _serviceComboVariants = serviceComboVariants.Select(p => new ServiceComboVariantDTO
                (
                    p.ServiceComboId,
                    p.BreedId,
                    (breedMapping != null && breedMapping.TryGetValue(p.BreedId, out var name)) ? name : string.Empty,
                    p.WeightRange,
                    p.ComboPrice,
                    p.EstimateTime,
                    p.IsVisible
                )).ToList();

                return (null, _serviceComboVariants);
            }

            return (null, null);
        }
    }
}
