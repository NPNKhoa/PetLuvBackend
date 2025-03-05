namespace BookingApi.Domain.Entities
{
    public class ServiceBookingDetail
    {
        public Guid ServiceId { get; set; }
        public Guid BreedId { get; set; }
        public string PetWeightRange { get; set; }
        public Guid BookingId { get; set; }
        public string ServiceItemName { get; set; } = string.Empty;
        public decimal BookingItemPrice { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
