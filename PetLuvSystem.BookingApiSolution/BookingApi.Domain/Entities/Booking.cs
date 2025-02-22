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
        public string TotalEstimateTime { get; set; } = string.Empty;

        public Guid BookingTypeId { get; set; }
        public virtual BookingType BookingType { get; set; }
        public Guid BookingStatusId { get; set; }
        public virtual BookingStatus BookingStatus { get; set; }

        public Guid PaymentStatusId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PetId { get; set; }

        public RoomBookingItem? RoomBookingItem { get; set; }
        public virtual ICollection<ServiceBookingDetail>? ServiceBookingDetails { get; set; }
        public virtual ICollection<ServiceComboBookingDetail>? ServiceComboBookingDetails { get; set; }
    }
}
