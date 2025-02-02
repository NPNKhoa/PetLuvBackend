namespace PetApi.Application.DTOs.PetDTOs
{
    public record BriefPetDTO
    (
        Guid PetId,
        string PetName,
        bool IsVisible
    );
}
