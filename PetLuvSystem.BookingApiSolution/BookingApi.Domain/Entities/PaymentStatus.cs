namespace BookingApi.Domain.Entities
{
    public class PaymentStatus
    {
        public Guid PaymmentStatusId { get; set; }
        public string PaymentStatusName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
