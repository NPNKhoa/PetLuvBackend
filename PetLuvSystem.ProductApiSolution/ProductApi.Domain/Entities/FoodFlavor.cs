namespace ProductApi.Domain.Entities
{
    public class FoodFlavor
    {
        public Guid FlavorId { get; set; }
        public string Flavor { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<FoodVariant> FoodVariants { get; set; }
    }
}
