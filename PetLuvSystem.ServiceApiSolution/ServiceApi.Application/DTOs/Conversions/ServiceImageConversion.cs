using ServiceApi.Application.DTOs.ServiceImageDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceImageConversion
    {
        public static ServiceImage ToEntity(ServiceImageDTO dto) => new()
        {
            ServiceImagePath = dto.ServiceImagePath,
            ServiceId = dto.ServiceId,
        };

        public static (ServiceImageDTO?, IEnumerable<ServiceImageDTO>?) FromEntity(ServiceImage? serviceImage, IEnumerable<ServiceImage>? serviceImages)
        {
            if (serviceImage is not null && serviceImages is null)
            {
                var singleServiceImage = new ServiceImageDTO
                (
                    serviceImage.ServiceImagePath,
                    serviceImage.ServiceId
                );
                return (singleServiceImage, null);
            }

            if (serviceImages is not null && serviceImage is null)
            {
                var _serviceImages = serviceImages.Select(p => new ServiceImageDTO(
                    p.ServiceImagePath,
                    p.ServiceId
                )).ToList();

                return (null, _serviceImages);
            }

            return (null, null);
        }
    }
}
