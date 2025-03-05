using BookingApi.Application.DTOs.ExternalEntities;

namespace BookingApi.Application.Utils
{
    public class PaymentStatusMapping
    {
        public bool Flag { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<PaymentStatus> Data { get; set; }
    }
}
