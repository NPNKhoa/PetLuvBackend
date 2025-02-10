namespace ProductApi.Domain.Entities
{
    public class FoodSize
    {
        public Guid SizeId { get; set; }
        public string Size { get; set; }
        public bool Isvisible { get; set; }

        public ICollection<FoodVariant> FoodVariants { get; set; }
    }
}
