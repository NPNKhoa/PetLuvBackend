namespace PetLuvSystem.SharedLibrary.Contracts.Messages
{
    public class CreateaPaymentCommand
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
    }
}
