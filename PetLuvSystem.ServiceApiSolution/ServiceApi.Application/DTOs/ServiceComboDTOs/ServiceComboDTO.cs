using ServiceApi.Application.DTOs.ServiceComboVariantDTOs;
using ServiceApi.Application.DTOs.ServiceDTOs;
using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceComboDTOs
{
    public record ServiceComboDTO
    (
        [Required] Guid ServiceComboId,
        [Required] string? ServiceComboName,
        [Required] string? ServiceComboDesc,
        [Required] ICollection<ServiceComboVariantDTO>? ComboVariants,
        [Required] ICollection<ServiceDTO>? Services,
        bool IsVisible = false
    );
}
