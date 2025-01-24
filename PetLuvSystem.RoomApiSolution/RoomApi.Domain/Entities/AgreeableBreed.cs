namespace RoomApi.Domain.Entities
{
    public class AgreeableBreed
    {
        public Guid RoomTypeId { get; set; }
        public Guid BreedId { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}
