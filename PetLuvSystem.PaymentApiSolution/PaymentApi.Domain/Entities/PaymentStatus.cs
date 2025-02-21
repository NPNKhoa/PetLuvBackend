namespace PaymentApi.Domain.Entities
{
    public class PaymentStatus
    {
        public Guid PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
