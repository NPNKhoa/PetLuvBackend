namespace PetApi.Application.DTOs.PetBreedDTOs
{
    public record BriefPetBreedDTO
    (
        Guid BreedId,
        string BreedName,
        string BreedDesc,
        string IllustrationImage,
        bool IsVisible,
        string PetTypeName
    );
}
