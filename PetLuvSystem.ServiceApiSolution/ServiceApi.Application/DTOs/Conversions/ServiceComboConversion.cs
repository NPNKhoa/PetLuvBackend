﻿using ServiceApi.Application.DTOs.ServiceComboDTOs;
using ServiceApi.Application.DTOs.ServiceComboVariantDTOs;
using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Application.DTOs.ServiceVariantDTOs;
using ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceComboConversion
    {
        public static ServiceCombo ToEntity(ServiceComboDTO dto) => new()
        {
            ServiceComboName = dto.ServiceComboName,
            ServiceComboDesc = dto.ServiceComboDesc,
            IsVisible = dto.IsVisible,
            ServiceComboMappings = dto.Services?.Select(p => new ServiceComboMapping
            {
                ServiceId = p.ServiceId,
                ServiceComboId = dto.ServiceComboId,
            }).ToList(),
        };

        public static ServiceCombo ToEntity(CreateUpdateServiceComboDTO dto) => new()
        {
            ServiceComboId = Guid.NewGuid(),
            ServiceComboName = dto.ServiceComboName,
            ServiceComboDesc = dto.ServiceComboDesc,
            IsVisible = dto.IsVisible,
        };

        public static (ServiceComboDTO?, ICollection<ServiceComboDTO>?) FromEntity(ServiceCombo? serviceCombo, ICollection<ServiceCombo>? serviceCombos, Dictionary<Guid, string>? breedMapping = null)
        {
            if (serviceCombo is not null && serviceCombos is null)
            {
                var singleServiceCombo = new ServiceComboDTO(
                    serviceCombo.ServiceComboId,
                    serviceCombo.ServiceComboName,
                    serviceCombo.ServiceComboDesc,
                    serviceCombo.ServiceComboVariants?.Select(x => new ServiceComboVariantDTO(
                        x.ServiceComboId,
                        x.BreedId,
                        (breedMapping != null && breedMapping.TryGetValue(x.BreedId, out var name)) ? name : string.Empty,
                        x.WeightRange,
                        x.ComboPrice,
                        x.EstimateTime,
                        x.IsVisible
                    )).ToList(),
                    serviceCombo.ServiceComboMappings?.Select(x => new ServiceDTO(
                        x.ServiceId,
                        x.Service is not null ? x.Service.ServiceName : "",
                        x.Service is not null ? x.Service.ServiceDesc : "",
                        x.Service is not null ? x.Service.IsVisible : false,
                        x.Service is not null ? x.Service.ServiceTypeId : Guid.Empty,
                        x.Service is not null && x.Service.ServiceType is not null ? x.Service.ServiceType.ServiceTypeName : "",
                        x.Service?.ServiceImages?.Select(s => s.ServiceImagePath).ToList()!,
                        new List<ServiceVariantDTO>(),
                        new List<WalkDogServiceVariantDTO>()
                    )).ToList(),
                    serviceCombo.IsVisible
                );

                return (singleServiceCombo, null);
            }

            if (serviceCombos is not null && serviceCombo is null)
            {
                var _serviceCombos = serviceCombos.Select(p => new ServiceComboDTO(
                    p.ServiceComboId,
                    p.ServiceComboName,
                    p.ServiceComboDesc,
                    p.ServiceComboVariants?.Select(x => new ServiceComboVariantDTO(
                        x.ServiceComboId,
                        x.BreedId,
                        (breedMapping != null && breedMapping.TryGetValue(x.BreedId, out var name)) ? name : string.Empty,
                        x.WeightRange,
                        x.ComboPrice,
                        x.EstimateTime,
                        x.IsVisible
                    )).ToList(),
                    p.ServiceComboMappings?.Select(x => new ServiceDTO(
                        x.ServiceId,
                        x.Service is not null ? x.Service.ServiceName : "",
                        x.Service is not null ? x.Service.ServiceDesc : "",
                        x.Service is not null ? x.Service.IsVisible : false,
                        x.Service is not null ? x.Service.ServiceTypeId : Guid.Empty,
                        x.Service is not null && x.Service.ServiceType is not null ? x.Service.ServiceType.ServiceTypeName : "",
                        x.Service?.ServiceImages?.Select(s => s.ServiceImagePath).ToList()!,
                        new List<ServiceVariantDTO>(),
                        new List<WalkDogServiceVariantDTO>()
                    )).ToList(),
                    p.IsVisible
                )).ToList();

                return (null, _serviceCombos);
            }

            return (null, null);
        }
    }
}
