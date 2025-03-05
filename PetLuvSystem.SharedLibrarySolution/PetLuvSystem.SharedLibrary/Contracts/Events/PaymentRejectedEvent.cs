namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class PaymentRejectedEvent
    {
        public Guid BookingId { get; set; }
        public string Reason { get; set; }
        public DateTime FailedAt { get; set; }
    }

}
