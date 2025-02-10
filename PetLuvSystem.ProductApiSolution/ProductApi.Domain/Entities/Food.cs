namespace ProductApi.Domain.Entities
{
    public class Food
    {
        public Guid FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDesc { get; set; }
        public string Brand { get; set; }
        public string Ingredient { get; set; }
        public string Origin { get; set; }
        public string AgeRange { get; set; }
        public decimal CountInStock { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<FoodImage> FoodImages { get; set; }
        public virtual ICollection<FoodVariant> FoodVariants { get; set; }
    }
}
