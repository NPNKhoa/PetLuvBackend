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
            IsVisible = dto.IsVisible,
            ServiceCombo = dto.ServiceCombo,
        };

        public static ServiceComboVariant ToEntity(CreateUpdateServiceComboVariantDTO dto) => new()
        {
            ServiceComboId = dto.ServiceComboId,
            BreedId = dto.BreedId,
            WeightRange = dto.WeightRange,
            ComboPrice = dto.ComboPrice,
            IsVisible = dto.IsVisible,
            ServiceCombo = null
        };

        public static (ServiceComboVariantDTO?, IEnumerable<ServiceComboVariantDTO>?) FromEntity(ServiceComboVariant? serviceComboVariant, IEnumerable<ServiceComboVariant>? serviceComboVariants)
        {
            if (serviceComboVariant is not null && serviceComboVariants is null)
            {
                var singleServiceComboVariant = new ServiceComboVariantDTO
                (
                    serviceComboVariant.ServiceComboId,
                    serviceComboVariant.BreedId,
                    serviceComboVariant.WeightRange,
                    serviceComboVariant.ComboPrice,
                    serviceComboVariant.IsVisible,
                    serviceComboVariant.ServiceCombo
                );
                return (singleServiceComboVariant, null);
            }

            if (serviceComboVariants is not null && serviceComboVariant is null)
            {
                var _serviceComboVariants = serviceComboVariants.Select(p => new ServiceComboVariantDTO
                (
                    p.ServiceComboId,
                    p.BreedId,
                    p.WeightRange,
                    p.ComboPrice,
                    p.IsVisible,
                    p.ServiceCombo
                )).ToList();

                return (null, _serviceComboVariants);
            }

            return (null, null);
        }
    }
}
