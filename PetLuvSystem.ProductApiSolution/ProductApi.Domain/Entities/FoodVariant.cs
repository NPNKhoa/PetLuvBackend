namespace ProductApi.Domain.Entities
{
    public class FoodVariant
    {
        public Guid FoodId { get; set; }
        public Guid FlavorId { get; set; }
        public Guid SizeId { get; set; }
        public decimal Price { get; set; }
        public bool Isvisible { get; set; }

        public virtual Food Food { get; set; }
        public virtual FoodFlavor Flavor { get; set; }
        public virtual FoodSize Size { get; set; }
    }
}
