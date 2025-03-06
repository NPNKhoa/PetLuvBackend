namespace PetLuvSystem.SharedLibrary.Contracts.Messages
{
    public class CancelBookingCommand
    {
        public Guid BookingId { get; set; }
        public string Reason { get; set; }
    }
}
