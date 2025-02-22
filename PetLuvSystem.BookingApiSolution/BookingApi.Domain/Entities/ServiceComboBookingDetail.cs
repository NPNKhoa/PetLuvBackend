namespace BookingApi.Domain.Entities
{
    public class ServiceComboBookingDetail
    {
        public Guid ServiceComboId { get; set; }
        public Guid BookingId { get; set; }
        public string ServiceComboItemName { get; set; } = string.Empty;
        public decimal BookingItemPrice { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
