using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceComboDTOs
{
    public record CreateUpdateServiceComboDTO
    (
        [Required] string? ServiceComboName,
        [Required] string? ServiceComboDesc,
        [Required, Range(1, int.MaxValue)] decimal ComboPrice,
        [Required] ICollection<ServiceDTO.ServiceDTO>? Services,
        bool IsVisible = false
    );
}
