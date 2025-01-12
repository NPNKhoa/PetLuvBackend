using ServiceApi.Application.DTOs.ServiceVariantDTOs;
using ServiceApi.Application.DTOs.WalkDogServiceVariantDTOs;
using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Application.DTOs.ServiceDTOs
{
    public record ServiceDTO(
        [Required] Guid ServiceId,

        [Required, StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        string? ServiceName,

        [StringLength(500, ErrorMessage = "Service description cannot exceed 500 characters.")]
        string? ServiceDesc,
        [Required, Range(1, int.MaxValue)]
        bool IsVisible,
        [Required] Guid ServiceTypeId,
        string? ServiceTypeName,
        ICollection<string>? ServiceImageUrls,
        ICollection<ServiceVariantDTO>? ServicePrices,
        ICollection<WalkDogServiceVariantDTO>? WalkDogServicePrices
    );
}
