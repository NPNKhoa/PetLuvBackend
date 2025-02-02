namespace PetApi.Application.DTOs.PetTypeDTOs
{
    public record BriefPetTypeDTO
    (
        Guid PetTypeId,
        string PetTypeName,
        bool IsVisible
    );
}
