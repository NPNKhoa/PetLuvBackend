namespace BookingApi.Domain.Entities
{
    public class BookingStatus
    {
        public Guid BookingStatusId { get; set; }
        public string BookingStatusName { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
