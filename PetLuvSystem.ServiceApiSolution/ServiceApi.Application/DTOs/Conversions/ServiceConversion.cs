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
            ServiceImages = dto.ServiceImageUrls?.Select(p => new ServiceImage
            {
                ServiceImagePath = p,
                ServiceId = dto.ServiceId
            }).ToList(),
            ServiceVariants = dto.ServiceVariants?.Select(p => new ServiceVariant
            {
                ServiceId = p.ServiceId,
                BreedId = p.BreedId,
                PetWeightRange = p.PetWeightRange,
                Price = p.Price
            }).ToList(),
            WalkDogServiceVariants = dto.WalkDogServiceVariants?.Select(p => new WalkDogServiceVariant
            {
                ServiceId = p.ServiceId,
                PricePerPeriod = p.PricePerPeriod
            }).ToList()
        };

        public static Service ToEntity(CreateServiceDTO dto) => new()
        {
            ServiceId = Guid.NewGuid(),
            ServiceName = dto.ServiceName,
            ServiceDesc = dto.ServiceDesc,
            IsVisible = dto.IsVisible,
            ServiceTypeId = dto.ServiceTypeId
        };

        public static Service ToEntity(UpdateServiceDTO dto) => new()
        {
            ServiceId = Guid.NewGuid(),
            ServiceName = dto.ServiceName ?? null,
            ServiceDesc = dto.ServiceDesc ?? null,
            IsVisible = dto.IsVisible.HasValue && dto.IsVisible.Value,
            ServiceTypeId = dto.ServiceTypeId ?? Guid.Empty
        };


        public static (ServiceDTO?, IEnumerable<ServiceDTO>?) FromEntity(Service? service, IEnumerable<Service>? services, Dictionary<Guid, string>? breedMapping = null)
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
                    service.ServiceType?.ServiceTypeName,
                    service.ServiceImages?.Select(p => p.ServiceImagePath).ToList()!,
                    service.ServiceVariants?.Select(p => new ServiceVariantDTO
                    (
                        p.ServiceId,
                        p.BreedId,
                        (breedMapping != null && breedMapping.TryGetValue(p.BreedId, out var name)) ? name : string.Empty,
                        p.PetWeightRange!,
                        p.Price,
                        p.EstimateTime,
                        p.IsVisible
                    )).ToList(),
                    service.WalkDogServiceVariants?.Select(p => new WalkDogServiceVariantDTO
                    (
                        p.ServiceId,
                        p.BreedId,
                        p.PricePerPeriod,
                        p.IsVisible
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
                    p.ServiceType?.ServiceTypeName,
                    p.ServiceImages?.Select(img => img.ServiceImagePath).ToList()!,
                    p.ServiceVariants?.Select(variant => new ServiceVariantDTO
                    (
                        variant.ServiceId,
                        variant.BreedId,
                        (breedMapping != null && breedMapping.TryGetValue(variant.BreedId, out var name)) ? name : string.Empty,
                        variant.PetWeightRange!,
                        variant.Price,
                        variant.EstimateTime,
                        variant.IsVisible
                    )).ToList() ?? new List<ServiceVariantDTO>(),
                    p.WalkDogServiceVariants?.Select(walkVariant => new WalkDogServiceVariantDTO
                    (
                        walkVariant.ServiceId,
                        walkVariant.BreedId,
                        walkVariant.PricePerPeriod,
                        walkVariant.IsVisible
                    )).ToList() ?? new List<WalkDogServiceVariantDTO>()
                )).ToList();

                return (null, _services);
            }


            return (null, null);
        }
    }
}
