using ServiceApi.Application.DTOs.ServiceDTOs;
using ServiceApi.Application.DTOs.ServiceVariantDTOs;
using ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceConversion
    {
        public static Service ToEntity(ServiceDTO dto) => new()
        {
            ServiceId = dto.ServiceId,
            ServiceName = dto.ServiceName,
            ServiceDesc = dto.ServiceDesc,
            IsVisible = dto.IsVisible,
            ServiceTypeId = dto.ServiceTypeId,
            ServiceType = new ServiceType
            {
                ServiceTypeId = dto.ServiceTypeId,
                ServiceTypeName = dto.ServiceTypeName
            },
            ServiceImages = dto.ServiceImageUrls?.Select(p => new ServiceImage
            {
                ServiceImagePath = p,
                ServiceId = dto.ServiceId
            }).ToList(),
            ServiceVariants = dto.ServicePrices?.Select(p => new ServiceVariant
            {
                ServiceId = dto.ServiceId,
                BreedId = p.BreedId,
                PetWeightRange = p.PetWeightRange,
                Price = p.ServicePrice
            }).ToList(),
            WalkDogServiceVariants = dto.WalkDogServicePrices?.Select(p => new WalkDogServiceVariant
            {
                ServiceId = dto.ServiceId,
                PricePerPeriod = p.PricePerPeriod,
                Service = p.Service
            }).ToList()
        };

        public static Service ToEntity(CreateUpdateServiceDTO dto) => new()
        {
            ServiceId = Guid.NewGuid(),
            ServiceName = dto.ServiceName,
            ServiceDesc = dto.ServiceDesc,
            ServiceTypeId = dto.ServiceTypeId,
        };

        public static (ServiceDTO?, IEnumerable<ServiceDTO>?) FromEntity(Service? service, IEnumerable<Service>? services)
        {
            if (service is not null && services is null)
            {
                var singleService = new ServiceDTO
                (
                    service.ServiceId,
                    service.ServiceName,
                    service.ServiceDesc,
                    service.IsVisible,
                    service.ServiceTypeId,
                    service.ServiceType.ServiceTypeName,
                    service?.ServiceImages?.Select(p => p.ServiceImagePath).ToList()!,
                    service?.ServiceVariants?.Select(p => new ServiceVariantDTO
                    (
                        p.ServiceId,
                        p.BreedId,
                        p.PetWeightRange!,
                        p.Price
                    )).ToList(),
                    service?.WalkDogServiceVariants?.Select(p => new WalkDogServiceVariantDTO
                    (
                        p.ServiceId,
                        p.PricePerPeriod,
                        p.Service
                    )).ToList()
                );
                return (singleService, null);
            }

            if (services is not null && service is null)
            {
                var _services = services.Select(p => new ServiceDTO
                (
                    p.ServiceId,
                    p.ServiceName,
                    p.ServiceDesc,
                    p.IsVisible,
                    p.ServiceTypeId,
                    p.ServiceType.ServiceTypeName,
                    p.ServiceImages?.Select(p => p.ServiceImagePath).ToList()!,
                    p.ServiceVariants?.Select(p => new ServiceVariantDTO
                    (
                        p.ServiceId,
                        p.BreedId,
                        p.PetWeightRange!,
                        p.Price
                    )).ToList(),
                    p.WalkDogServiceVariants?.Select(p => new WalkDogServiceVariantDTO
                    (
                        p.ServiceId,
                        p.PricePerPeriod,
                        p.Service
                    )).ToList()
                )).ToList();

                return (null, _services);
            }

            return (null, null);
        }
    }
}
