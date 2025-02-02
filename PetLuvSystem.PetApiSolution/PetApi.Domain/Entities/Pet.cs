namespace PetApi.Domain.Entities
{
    public class Pet
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public DateTime PetDateOfBirth { get; set; }
        public bool PetGender { get; set; }
        public string PetFurColor { get; set; }
        public double PetWeight { get; set; }
        public string? PetDesc { get; set; }
        public string? PetFamilyRole { get; set; }
        public bool IsVisible { get; set; }

        public Guid? ParentPetId { get; set; }
        public virtual Pet ParentPet { get; set; }
        public Guid BreedId { get; set; }
        public virtual PetBreed PetBreed { get; set; }
        public Guid? CustomerId { get; set; }

        public virtual ICollection<PetImage> PetImagePaths { get; set; }
        public virtual ICollection<Pet> ChildrenPets { get; set; }
        public virtual ICollection<PetHealthBook> PetHealthBooks { get; set; }
    }
}
