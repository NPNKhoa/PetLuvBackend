using ServiceApi.Application.DTOs.ServiceComboMappingDTOs;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.Conversions
{
    public static class ServiceComboMappingConversion
    {
        public static ServiceComboMapping ToEntity(ServiceComboMappingDTO entity) => new()
        {
            ServiceId = entity.ServiceId,
            ServiceComboId = entity.ServiceComboId,
            Service = entity.Service,
            ServiceCombo = entity.ServiceCombo
        };

        public static ServiceComboMapping ToEntity(CreateUpdateServiceComboMappingDTO entity) => new()
        {
            ServiceId = entity.ServiceId,
            ServiceComboId = entity.ServiceComboId,
        };

        public static (ServiceComboMappingDTO?, IEnumerable<ServiceComboMappingDTO>?) FromEntity(ServiceComboMapping? entity, IEnumerable<ServiceComboMapping>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDto = new ServiceComboMappingDTO(entity.ServiceId, entity.ServiceComboId, entity.Service!, entity.ServiceCombo!);

                return (singleDto, null);
            }

            if (entities is not null && entity is null)
            {
                var dtoList = entities.Select(e => new ServiceComboMappingDTO(e.ServiceId, e.ServiceComboId, e.Service!, e.ServiceCombo!)).ToList();

                return (null, dtoList);
            }

            return (null, null);
        }
    }
}
