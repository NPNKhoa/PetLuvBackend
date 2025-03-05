namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class BookingConfirmationEmailSentEvent
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public double AmountPaid { get; set; }
        public DateTime SentAt { get; set; }
    }

}
