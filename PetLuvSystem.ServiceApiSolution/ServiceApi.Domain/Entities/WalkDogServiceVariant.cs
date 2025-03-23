using System.Text.Json.Serialization;

namespace ServiceApi.Domain.Entities
{
    public class WalkDogServiceVariant
    {
        public Guid ServiceId { get; set; }
        public Guid BreedId { get; set; }
        public decimal PricePerPeriod { get; set; }
        public int Period { get; set; }
        public bool IsVisible { get; set; }

        [JsonIgnore]
        public virtual Service? Service { get; set; }
    }
}
