using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.DTOs.ServiceComboMappingDTOs
{
    public record ServiceComboMappingDTO
    (
        Guid ServiceId,
        Guid ServiceComboId,

        Service Service,
        ServiceCombo ServiceCombo
    );
}
