namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class BookingPaymentEmailFailedEvent
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string Reason { get; set; }
        public DateTime FailedAt { get; set; }
    }
}
