using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.PetDTOs
{
    public record PetDTO
    (
        Guid PetId,
        string PetName,
        DateTime PetDateOfBirth,
        bool PetGender,
        string PetFurColor,
        double PetWeight,
        string PetDesc,
        string? PetFamilyRole,
        bool IsVisible,

        Guid? ParentPetId,
        BriefPetDTO? ParentPet,
        Guid BreedId,
        string BreedName,
        Guid? CustomerId,

        ICollection<PetImage> PetImagePaths,
        ICollection<Pet> ChildrenPets,
        ICollection<PetHealthBook> PetHealthBooks
    );
}
