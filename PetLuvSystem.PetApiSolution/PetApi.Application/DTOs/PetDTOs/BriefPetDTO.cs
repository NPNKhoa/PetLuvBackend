namespace PetApi.Application.DTOs.PetDTOs
{
    public record BriefPetDTO
    (
        Guid PetId,
        string PetName,
        string? PetImagePath,
        bool IsVisible
    );
}
