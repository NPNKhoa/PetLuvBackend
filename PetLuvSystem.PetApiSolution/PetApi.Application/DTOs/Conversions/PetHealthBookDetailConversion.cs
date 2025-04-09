using PetApi.Application.DTOs.PetHealthBookDetailDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class PetHealthBookDetailConversion
    {
        public static PetHealthBookDetail ToEntity(PetHealthBookDetailDTO dto) => new()
        {
            HealthBookDetailId = dto.HealthBookDetailId,
            PetHealthNote = dto.PetHealthNote,
            TreatmentName = dto.TreatmentName,
            TreatmentDesc = dto.TreatmentDesc,
            TreatmentProof = dto.TreatmentProof,
            VetName = dto.VetName,
            VetDegree = dto.VetDegree,
            UpdatedDate = dto.UpdatedDate,
            HealthBookId = dto.HealthBookId
        };

        public static PetHealthBookDetail ToEntity(CreateUpdatePetHealthBookDetailDTO dto, string treatmaneProofPath, string vetDegreeProofPath) => new()
        {
            HealthBookDetailId = Guid.NewGuid(),
            PetHealthNote = dto.PetHealthNote,
            TreatmentName = dto.TreatmentName,
            TreatmentDesc = dto.TreatmentDesc!,
            TreatmentProof = treatmaneProofPath,
            VetName = dto.VetName,
            VetDegree = vetDegreeProofPath,
            UpdatedDate = dto.UpdatedDate,
            HealthBookId = dto.HealthBookId
        };

        public static PetHealthBookDetail ToEntity(UpdatePetHealthBookDetailDTO dto, string treatmaneProofPath, string vetDegreeProofPath) => new()
        {
            HealthBookDetailId = Guid.NewGuid(),
            PetHealthNote = dto.PetHealthNote,
            TreatmentName = dto.TreatmentName,
            TreatmentDesc = dto.TreatmentDesc!,
            TreatmentProof = treatmaneProofPath,
            VetName = dto.VetName,
            VetDegree = vetDegreeProofPath,
            UpdatedDate = dto.UpdatedDate,
        };

        public static (PetHealthBookDetailDTO?, IEnumerable<PetHealthBookDetailDTO>?) FromEntity(PetHealthBookDetail? entity, IEnumerable<PetHealthBookDetail> entities)
        {

            if (entity is not null && entities is null)
            {
                return (new PetHealthBookDetailDTO(
                    entity.HealthBookDetailId,
                    entity.PetHealthNote,
                    entity.TreatmentName,
                    entity.TreatmentDesc,
                    entity.TreatmentProof,
                    entity.VetName,
                    entity.VetDegree,
                    entity.UpdatedDate,
                    entity.HealthBookId
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(e => new PetHealthBookDetailDTO(
                    e.HealthBookDetailId,
                    e.PetHealthNote,
                    e.TreatmentName,
                    e.TreatmentDesc,
                    e.TreatmentProof,
                    e.VetName,
                    e.VetDegree,
                    e.UpdatedDate,
                    e.HealthBookId
                )).ToList());
            }

            return (null, null);
        }
    }
}
