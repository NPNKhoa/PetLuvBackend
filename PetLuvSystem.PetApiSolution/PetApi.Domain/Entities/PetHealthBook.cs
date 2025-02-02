namespace PetApi.Domain.Entities
{
    public class PetHealthBook
    {
        public Guid HealthBookId { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<PetHealthBookDetail> PetHealthBookDetails { get; set; }
    }
}
