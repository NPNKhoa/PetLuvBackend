namespace ServiceApi.Domain.Entities
{
    public class WalkDogServicePrice
    {
        public Guid ServiceId { get; set; }
        public decimal PricePerPeriod { get; set; }

        public Service? Service { get; set; }
    }
}
