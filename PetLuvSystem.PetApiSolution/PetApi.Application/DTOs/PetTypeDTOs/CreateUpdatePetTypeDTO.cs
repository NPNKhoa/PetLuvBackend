using System.ComponentModel.DataAnnotations;

namespace PetApi.Application.DTOs.PetTypeDTOs
{
    public record CreateUpdatePetTypeDTO
    (
        [Required, MaxLength(100)]
        string PetTypeName,
        [Required, MaxLength(500)]
        string PetTypeDesc,
        bool IsVisible
    );
}
