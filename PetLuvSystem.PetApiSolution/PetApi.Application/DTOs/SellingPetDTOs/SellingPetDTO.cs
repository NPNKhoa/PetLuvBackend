using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Application.DTOs.PetDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.SellingPetDTOs
{
    public record SellingPetDTO
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
        BriefPetBreedDTO PetBreed,

        ICollection<string> ImagePaths,
        ICollection<BriefPetDTO> ChildrenFromMother,
        ICollection<BriefPetDTO> ChildrenFromFather,
        PetHealthBook PetHealthBook
    );
}
