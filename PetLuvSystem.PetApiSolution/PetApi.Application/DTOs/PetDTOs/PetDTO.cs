using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.PetDTOs
{
    public record PetDTO
    (
        Guid PetId,
        string PetName,
        DateTime PetDateOfBirth,
        bool PetGender,
        string? PetFurColor,
        double PetWeight,
        string? PetDesc,
        string? PetFamilyRole,
        bool IsVisible,

        Guid? MotherId,
        Guid? FatherId,
        BriefPetDTO? Mother,
        BriefPetDTO? Father,
        Guid BreedId,
        string BreedName,
        string petTypeName,
        Guid? CustomerId,

        ICollection<PetImage> PetImagePaths,
        ICollection<BriefPetDTO> ChildrenFromMother,
        ICollection<BriefPetDTO> ChildrenFromFather
    );
}
