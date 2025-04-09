namespace PetApi.Application.DTOs.PetHealthBookDetailDTOs
{
    public record UpdatePetHealthBookDetailDTO
    (
        string PetHealthNote,
        string TreatmentName,
        string? TreatmentDesc,
        string VetName,
        DateTime UpdatedDate
    );
}
