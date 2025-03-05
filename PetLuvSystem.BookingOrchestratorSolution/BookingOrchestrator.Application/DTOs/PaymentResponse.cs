namespace BookingOrchestrator.Domain.DTOs
{
    public class PaymentResponse
    {
        public bool Flag { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

}
