namespace BookingApi.Domain.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public string? BookingNote { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public Guid BookingTypeId { get; set; }
        public Guid PaymentStatusId { get; set; }
        public Guid BookingStatusId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PetId { get; set; }
        public Guid? RoomBookingId { get; set; }

        public virtual BookingType BookingType { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual BookingStatus BookingStatus { get; set; }
    }
}
