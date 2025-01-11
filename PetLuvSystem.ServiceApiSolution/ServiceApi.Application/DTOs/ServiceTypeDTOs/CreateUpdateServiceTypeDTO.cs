using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceTypeDTOs
{
    public record CreateUpdateServiceTypeDTO
    (
        [Required] string? ServiceTypeName,
        string? ServiceTypeDesc,
        bool IsVisible = false
    );
}
