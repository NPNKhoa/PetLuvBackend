namespace PaymentApi.Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string? ResponseData { get; set; }

        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public Guid PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
