namespace BookingApi.Application.DTOs.ExternalEntities
{
    public class PetDTO
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public double PetWeight { get; set; }
        public bool IsVisible { get; set; }

        public Guid BreedId { get; set; }
    }
}
