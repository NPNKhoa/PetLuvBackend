using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceDTOs
{
    public record CreateUpdateServiceDTO
    (
        [Required, StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        string? ServiceName,

        [StringLength(500, ErrorMessage = "Service description cannot exceed 500 characters.")]
        string? ServiceDesc,
        [Required, Range(1, int.MaxValue)]
        bool IsVisible,
        [Required] Guid ServiceTypeId
    );
}
