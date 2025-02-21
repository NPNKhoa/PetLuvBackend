namespace PaymentApi.Domain.Entities
{
    public class PaymentMethod
    {
        public Guid PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
