namespace PetApi.Application.DTOs.PetDTOs
{
    public record UpdatePetFamilyDTO
    (
        Guid? MotherId,
        Guid? FatherId,
        ICollection<Guid>? ChildrenIds,
        string PetFamilyRole
    );
}
