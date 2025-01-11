using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceComboDTOs
{
    public record ServiceComboDTO
    (
        [Required] Guid ServiceComboId,
        [Required] string? ServiceComboName,
        [Required] string? ServiceComboDesc,
        [Required] ICollection<ServiceComboPriceDTOs.ServiceComboPriceDTO>? ComboPrices,
        [Required] ICollection<ServiceDTO.ServiceDTO>? Services,
        bool IsVisible = false
    );
}
