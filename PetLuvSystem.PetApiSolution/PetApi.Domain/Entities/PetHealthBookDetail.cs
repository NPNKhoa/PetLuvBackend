namespace PetApi.Domain.Entities
{
    public class PetHealthBookDetail
    {
        public Guid HealthBookDetailId { get; set; }
        public string PetHealthNote { get; set; }
        public string TreatmentName { get; set; }
        public string TreatmentDesc { get; set; }
        public string TreatmentProof { get; set; }
        public string VetName { get; set; }
        public string VetDegree { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Guid HealthBookId { get; set; }
        public virtual PetHealthBook PetHealthBook { get; set; }
    }
}
