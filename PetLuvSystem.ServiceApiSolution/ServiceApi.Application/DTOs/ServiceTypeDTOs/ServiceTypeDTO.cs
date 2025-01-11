using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceTypeDTOs
{
    public record ServiceTypeDTO
    (
        [Required] Guid ServiceTypeId,
        [Required] string? ServiceTypeName,
        string? ServiceTypeDesc,
        bool IsVisible = false
    );
}
