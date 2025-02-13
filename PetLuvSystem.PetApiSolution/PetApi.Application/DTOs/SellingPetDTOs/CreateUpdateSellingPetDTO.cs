using System.ComponentModel.DataAnnotations;

namespace PetApi.Application.DTOs.SellingPetDTOs
{
    public record CreateUpdateSellingPetDTO
    (
        [Required]
        string PetName,
        [Required]
        DateTime PetDateOfBirth,
        [Required]
        bool PetGender,
        string? PetFurColor,
        [Required]
        double PetWeight,
        string? PetDesc,
        string? PetFamilyRole,
        [Required]
        bool IsVisible,

        Guid? MotherId,
        Guid? FatherId,
        [Required]
        Guid BreedId
    );
}
