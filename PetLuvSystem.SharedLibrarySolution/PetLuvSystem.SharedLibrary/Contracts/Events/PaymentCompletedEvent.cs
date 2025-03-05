namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class PaymentCompletedEvent
    {
        public Guid BookingId { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
