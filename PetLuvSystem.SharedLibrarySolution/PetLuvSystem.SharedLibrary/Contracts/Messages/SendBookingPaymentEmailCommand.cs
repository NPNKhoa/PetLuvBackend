namespace PetLuvSystem.SharedLibrary.Contracts.Messages
{
    public class SendBookingPaymentEmailCommand
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string PaymentUrl { get; set; }
    }
}
