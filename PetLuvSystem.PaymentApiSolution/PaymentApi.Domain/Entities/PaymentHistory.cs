namespace PaymentApi.Domain.Entities
{
    public class PaymentHistory
    {
        public Guid PaymentHistoryId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public Guid PaymentStatusId { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
    }
}
