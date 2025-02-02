using PetApi.Application.DTOs.PetDTOs;

namespace PetApi.Application.DTOs.PetBreedDTOs
{
    public record PetBreedDTO
    (
        Guid BreedId,
        string BreedName,
        string BreedDesc,
        string IllustrationImage,
        bool IsVisible,
        Guid PetTypeId,
        string PetTypeName,
        IEnumerable<BriefPetDTO> Pets
    );
}
