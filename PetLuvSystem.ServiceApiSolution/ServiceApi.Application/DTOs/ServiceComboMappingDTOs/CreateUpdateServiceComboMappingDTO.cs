namespace ServiceApi.Application.DTOs.ServiceComboMappingDTOs
{
    public record CreateUpdateServiceComboMappingDTO
    (
        Guid ServiceId,
        Guid ServiceComboId
    );
}
