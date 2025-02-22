namespace BookingApi.Domain.Entities
{
    public class ServiceBookingDetail
    {
        public Guid ServiceVariantId { get; set; }
        public Guid BookingId { get; set; }
        public string ServiceItemName { get; set; } = string.Empty;
        public decimal BookingItemPrice { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
