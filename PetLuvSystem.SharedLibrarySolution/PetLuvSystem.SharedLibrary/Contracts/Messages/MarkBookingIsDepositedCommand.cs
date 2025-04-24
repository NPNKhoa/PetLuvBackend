namespace PetLuvSystem.SharedLibrary.Contracts.Messages
{
    public class MarkBookingIsDepositedCommand
    {
        public Guid BookingId { get; set; }
        public Guid PaymentStatusId { get; set; }
        public Decimal DepositAmount { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
