namespace BookingApi.Application.DTOs.ExternalEntities
{
    public class PaymentStatus
    {
        public Guid PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public bool IsVisible { get; set; }
    }
}
