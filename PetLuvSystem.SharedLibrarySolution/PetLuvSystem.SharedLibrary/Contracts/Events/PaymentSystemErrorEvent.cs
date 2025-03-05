namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class PaymentSystemErrorEvent
    {
        public Guid BookingId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorAt { get; set; }
    }

}
