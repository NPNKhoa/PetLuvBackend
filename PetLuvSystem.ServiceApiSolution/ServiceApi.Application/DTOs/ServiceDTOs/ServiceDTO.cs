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
        bool IsVisible,
        string? ServiceTypeName,
        ICollection<string>? ServiceImageUrls,
        ICollection<ServiceVariantDTO>? ServiceVariants,
        ICollection<WalkDogServiceVariantDTO>? WalkDogServiceVariants
    );
}
