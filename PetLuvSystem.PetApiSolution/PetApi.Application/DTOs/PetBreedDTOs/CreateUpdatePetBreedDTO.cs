using Microsoft.AspNetCore.Http;

namespace PetApi.Application.DTOs.PetBreedDTOs
{
    public record CreateUpdatePetBreedDTO
    (
        string BreedName,
        string BreedDesc,
        bool IsVisible,
        IFormFile IllustrationImage,
        Guid PetTypeId
    );
}
