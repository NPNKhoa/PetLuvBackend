namespace ServiceApi.Domain.Entities
{
    public class WalkDogServiceVariant
    {
        public Guid ServiceId { get; set; }
        public Guid BreedId { get; set; }
        public decimal PricePerPeriod { get; set; }

        public virtual Service? Service { get; set; }
    }
}
