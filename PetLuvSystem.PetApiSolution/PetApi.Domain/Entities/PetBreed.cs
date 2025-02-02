namespace PetApi.Domain.Entities
{
    public class PetBreed
    {
        public Guid BreedId { get; set; }
        public string BreedName { get; set; }
        public string BreedDesc { get; set; }
        public string IllustrationImage { get; set; }
        public bool IsVisible { get; set; }

        public Guid PetTypeId { get; set; }
        public virtual PetType PetType { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
