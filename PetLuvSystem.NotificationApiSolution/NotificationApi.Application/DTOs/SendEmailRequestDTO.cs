namespace NotificationApi.Application.DTOs
{
    public class SendEmailRequestDTO
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public Guid BookingId { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
    }
}
