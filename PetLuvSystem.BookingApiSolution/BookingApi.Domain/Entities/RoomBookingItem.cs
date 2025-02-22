namespace BookingApi.Domain.Entities
{
    public class RoomBookingItem
    {
        public Guid BookingId { get; set; }
        public Guid RoomId { get; set; }
        public decimal ItemPrice { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
