using Microsoft.AspNetCore.Http;

namespace PetApi.Application.DTOs.PetHealthBookDetailDTOs
{
    public record CreateUpdatePetHealthBookDetailDTO
    (
        string PetHealthNote,
        string TreatmentName,
        string? TreatmentDesc,
        IFormFile? TreatmentProof,
        string VetName,
        IFormFile? VetDegree,
        DateTime UpdatedDate,
        Guid HealthBookId
    );
}
