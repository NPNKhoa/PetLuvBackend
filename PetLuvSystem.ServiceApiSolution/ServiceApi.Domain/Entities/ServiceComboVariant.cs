namespace ServiceApi.Domain.Entities
{
    public class ServiceComboVariant
    {
        public Guid ServiceComboId { get; set; }
        public Guid BreedId { get; set; }
        public string? WeightRange { get; set; }
        public decimal ComboPrice { get; set; }
        public int EstimateTime { get; set; }
        public bool IsVisible { get; set; }

        public virtual ServiceCombo? ServiceCombo { get; set; }
    }
}
