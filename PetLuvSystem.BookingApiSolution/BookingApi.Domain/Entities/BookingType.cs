namespace BookingApi.Domain.Entities
{
    public class BookingType
    {
        public Guid BookingTypeId { get; set; }
        public string BookingTypeName { get; set; }
        public string BookingTypeDesc { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
