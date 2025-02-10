using System.ComponentModel.DataAnnotations;

namespace ProductApi.Domain.Entities
{
    public class FoodImage
    {
        [Key]
        public string FoodImagePath { get; set; }
        public Guid FoodId { get; set; }
        public bool IsVisible { get; set; }

        public virtual Food Food { get; set; }
    }
}
