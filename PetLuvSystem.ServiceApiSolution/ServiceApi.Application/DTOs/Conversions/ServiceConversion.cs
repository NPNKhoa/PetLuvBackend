using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceConversion
    {
        public static Service ToEntity(ServiceDTO.ServiceDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            ServiceName = dto.ServiceName,
            ServiceDesc = dto.ServiceDesc,
            ServiceTypeId = dto.ServiceTypeId,
        };

        public static Service ToEntity(CreateUpdateServiceDTO dto) => new()
        {
            ServiceId = Guid.NewGuid(),
            ServiceName = dto.ServiceName,
            ServiceDesc = dto.ServiceDesc,
            ServiceTypeId = dto.ServiceTypeId,
        };

        public static (ServiceDTO.ServiceDTO?, IEnumerable<ServiceDTO.ServiceDTO>?) FromEntity(Service? service, IEnumerable<Service>? services)
        {
            if (service is not null && services is null)
            {
                var singleService = new ServiceDTO.ServiceDTO
                (
                    service.ServiceId,
                    service.ServiceName,
                    service.ServiceDesc,
                    service.IsVisible,
                    service.ServiceTypeId,
                    service.ServiceType.ServiceTypeName,
                    service.ServiceImages?.Select(p => p.ServiceImagePath).ToList()
                );
                return (singleService, null);
            }

            if (services is not null && service is null)
            {
                var _services = services.Select(p => new ServiceDTO.ServiceDTO
                (
                    p.ServiceId,
                    p.ServiceName,
                    p.ServiceDesc,
                    p.IsVisible,
                    p.ServiceTypeId,
                    p.ServiceType.ServiceTypeName,
                    p.ServiceImages?.Select(p => p.ServiceImagePath).ToList()
                )).ToList();

                return (null, _services);
            }

            return (null, null);
        }
    }
}
