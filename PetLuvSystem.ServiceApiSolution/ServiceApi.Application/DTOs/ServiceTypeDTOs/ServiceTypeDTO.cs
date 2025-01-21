using ServiceApi.Application.DTOs.ServiceDTOs;

namespace ServiceApi.Application.DTOs.ServiceTypeDTOs
{
    public record ServiceTypeDTO(
        Guid ServiceTypeId,
        string? ServiceTypeName,
        string? ServiceTypeDesc,
        bool IsVisible,
        ICollection<BriefServiceDTO>? Services
    );
}