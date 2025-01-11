using ServiceApi.Application.DTOs.ServiceComboDTOs;
using ServiceApi.Application.DTOs.ServiceComboPriceDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceComboConversion
    {
        public static ServiceCombo ToEntity(ServiceComboDTO dto) => new()
        {
            ServiceComboId = dto.ServiceComboId,
            ServiceComboName = dto.ServiceComboName,
            ServiceComboDesc = dto.ServiceComboDesc,
            IsVisible = dto.IsVisible,
            ServiceComboPrices = dto.ComboPrices?.Select(p => new ServiceComboPrice
            {
                ServiceComboId = dto.ServiceComboId,
                BreedId = p.BreedId,
                WeightRange = p.WeightRange,
                ComboPrice = p.ComboPrice
            }).ToList(),
            ServiceComboMappings = dto.Services?.Select(s => new ServiceComboMapping
            {
                ServiceId = s.ServiceId,
                ServiceComboId = dto.ServiceComboId
            }).ToList()
        };

        public static ServiceCombo ToEntity(CreateUpdateServiceComboDTO dto) => new()
        {
            ServiceComboId = Guid.NewGuid(),
            ServiceComboName = dto.ServiceComboName,
            ServiceComboDesc = dto.ServiceComboDesc,
            IsVisible = dto.IsVisible,
            ServiceComboMappings = dto.Services?.Select(s => new ServiceComboMapping
            {
                ServiceId = s.ServiceId,
                ServiceComboId = Guid.NewGuid()
            }).ToList()
        };

        public static (ServiceComboDTO?, IEnumerable<ServiceComboDTO>?) FromEntity(ServiceCombo? serviceCombo, IEnumerable<ServiceCombo>? serviceCombos)
        {
            if (serviceCombo is not null && serviceCombos is null)
            {
                var singleServiceCombo = new ServiceComboDTO
                (
                    serviceCombo.ServiceComboId,
                    serviceCombo.ServiceComboName,
                    serviceCombo.ServiceComboDesc,
                    serviceCombo.ServiceComboPrices?.Select(s => new ServiceComboPriceDTO
                    (
                        s.ServiceComboId,
                        s.BreedId,
                        s.WeightRange,
                        s.ComboPrice,
                        null
                    )).ToList() ?? new List<ServiceComboPriceDTO>(),
                    serviceCombo.ServiceComboMappings?.Select(m => new ServiceDTO.ServiceDTO
                    (
                        m.Service.ServiceId,
                        m.Service.ServiceName,
                        m.Service.ServiceDesc,
                        m.Service.IsVisible,
                        m.Service.ServiceTypeId,
                        m.Service.ServiceType.ServiceTypeName,
                        m.Service.ServiceImages?.Select(p => p.ServiceImagePath).ToList()
                    )).ToList() ?? new List<ServiceDTO.ServiceDTO>(),
                    serviceCombo.IsVisible
                );
                return (singleServiceCombo, null);
            }

            if (serviceCombos is not null && serviceCombo is null)
            {
                var _serviceCombos = serviceCombos.Select(p => new ServiceComboDTO
                (
                    p.ServiceComboId,
                    p.ServiceComboName,
                    p.ServiceComboDesc,
                    p.ServiceComboPrices?.Select(s => new ServiceComboPriceDTO
                    (
                        s.ServiceComboId,
                        s.BreedId,
                        s.WeightRange,
                        s.ComboPrice,
                        null
                    )).ToList() ?? new List<ServiceComboPriceDTO>(),
                    p.ServiceComboMappings?.Select(m => new ServiceDTO.ServiceDTO
                    (
                        m.Service.ServiceId,
                        m.Service.ServiceName,
                        m.Service.ServiceDesc,
                        m.Service.IsVisible,
                        m.Service.ServiceTypeId,
                        m.Service.ServiceType.ServiceTypeName,
                        m.Service.ServiceImages?.Select(p => p.ServiceImagePath).ToList()
                    )).ToList() ?? new List<ServiceDTO.ServiceDTO>(),
                    p.IsVisible
                )).ToList();

                return (null, _serviceCombos);
            }

            return (null, null);
        }
    }
}
