using PetApi.Application.DTOs.PetBreedDTOs;
using System.ComponentModel.DataAnnotations;

namespace PetApi.Application.DTOs.PetTypeDTOs
{
    public record PetTypeDTO
    (
        [Required]
        Guid PetTypeId,
        [Required, MaxLength(100)]
        string PetTypeName,
        [Required, MaxLength(500)]
        string PetTypeDesc,
        bool IsVisible,
        IEnumerable<BriefPetBreedDTO> PetBreeds
    );
}
