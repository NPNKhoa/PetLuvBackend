namespace ServiceApi.Domain.Entities
{
    public class WalkDogServiceVariant
    {
        public Guid ServiceId { get; set; }
        public decimal PricePerPeriod { get; set; }

        public Service? Service { get; set; }
    }
}
