using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Application.DTOs.ServiceTypeDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceTypeConversion
    {
        public static ServiceType ToEntity(ServiceTypeDTO dto) => new()
        {
            ServiceTypeId = dto.ServiceTypeId,
            ServiceTypeName = dto.ServiceTypeName,
            ServiceTypeDesc = dto.ServiceTypeDesc,
            IsVisible = dto.IsVisible,
        };

        public static ServiceType ToEntity(CreateUpdateServiceTypeDTO dto) => new()
        {
            ServiceTypeId = Guid.NewGuid(),
            ServiceTypeName = dto.ServiceTypeName,
            ServiceTypeDesc = dto.ServiceTypeDesc,
            IsVisible = dto.IsVisible,
        };

        public static (ServiceTypeDTO?, IEnumerable<ServiceTypeDTO>?) FromEntity(ServiceType? serviceType, IEnumerable<ServiceType>? serviceTypes)
        {
            if (serviceType is not null && serviceTypes is null)
            {
                var singleServiceType = new ServiceTypeDTO
                (
                    serviceType.ServiceTypeId,
                    serviceType.ServiceTypeName,
                    serviceType.ServiceTypeDesc,
                    serviceType.IsVisible,
                    serviceType.Services?.Select(s => new BriefServiceDTO(
                        s.ServiceTypeId,
                        s.ServiceName,
                        s.ServiceDesc,
                        s.IsVisible
                    )).ToList()!
                );
                return (singleServiceType, null);
            }

            if (serviceTypes is not null && serviceType is null)
            {
                var _serviceTypes = serviceTypes.Select(p => new ServiceTypeDTO(
                    p.ServiceTypeId,
                    p.ServiceTypeName,
                    p.ServiceTypeDesc,
                    p.IsVisible,
                    p.Services?.Select(s => new BriefServiceDTO(
                        s.ServiceTypeId,
                        s.ServiceName,
                        s.ServiceDesc,
                        s.IsVisible
                    )).ToList()!
                )).ToList();

                return (null, _serviceTypes);
            }

            return (null, null);
        }
    }
}
