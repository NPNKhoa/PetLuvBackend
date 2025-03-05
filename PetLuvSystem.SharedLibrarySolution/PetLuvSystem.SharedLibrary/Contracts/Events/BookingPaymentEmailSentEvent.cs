namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class BookingPaymentEmailSentEvent
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime SentAt { get; set; }
    }
}
