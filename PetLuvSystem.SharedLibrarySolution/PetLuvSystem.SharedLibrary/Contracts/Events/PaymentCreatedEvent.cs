namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class PaymentCreatedEvent
    {
        public Guid BookingId { get; set; }
        public string PaymentUrl { get; set; }
    }
}
