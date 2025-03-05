namespace PetLuvSystem.SharedLibrary.Contracts.Messages
{
    public class SendBookingConfirmationEmailCommand
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AmountPaid { get; set; }
    }

}
