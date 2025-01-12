namespace ServiceApi.Domain.Entities
{
    public class ServiceVariant
    {
        public Guid ServiceId { get; set; }
        public Guid BreedId { get; set; }
        public string? PetWeightRange { get; set; }
        public decimal Price { get; set; }

        public Service? Service { get; set; }
    }
}
