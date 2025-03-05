namespace BookingApi.Application.DTOs.ExternalEntities
{
    public class ServiceVariantDTO
    {
        public Guid ServiceId { get; set; }
        public Guid BreedId { get; set; }
        public string? PetWeightRange { get; set; }
        public decimal Price { get; set; }
        public int EstimateTime { get; set; }
        public bool IsVisible { get; set; }
    }
}
