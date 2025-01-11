namespace ServiceApi.Domain.Entities
{
    public class ServiceComboMapping
    {
        public Guid ServiceId { get; set; }
        public Guid ServiceComboId { get; set; }

        public virtual Service? Service { get; set; }
        public virtual ServiceCombo? ServiceCombo { get; set; }
    }
}
