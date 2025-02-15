namespace PetApi.Application.DTOs.PetHealthBookDetailDTOs
{
    public record PetHealthBookDetailDTO
    (
        Guid HealthBookDetailId,
        string PetHealthNote,
        string TreatmentName,
        string TreatmentDesc,
        string TreatmentProof,
        string VetName,
        string VetDegree,
        DateTime UpdatedDate,
        Guid HealthBookId
    );
}
