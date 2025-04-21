namespace RoomApi.Domain.Entities
{
    public class AgreeableBreed
    {
        public Guid BreedId { get; set; }
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
