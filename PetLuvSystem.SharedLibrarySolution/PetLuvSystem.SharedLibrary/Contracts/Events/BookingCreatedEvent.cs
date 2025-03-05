namespace PetLuvSystem.SharedLibrary.Contracts.Events
{
    public class BookingCreatedEvent
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public Guid PetId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
    }
}
