namespace PetApi.Domain.Entities
{
    public class PetType
    {
        public Guid PetTypeId { get; set; }
        public string PetTypeName { get; set; }
        public string PetTypeDesc { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<PetBreed> PetBreeds { get; set; }
    }
}
