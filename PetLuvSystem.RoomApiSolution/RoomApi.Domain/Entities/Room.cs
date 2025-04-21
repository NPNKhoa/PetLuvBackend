namespace RoomApi.Domain.Entities
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomDesc { get; set; }
        public decimal PricePerHour { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsVisible { get; set; }

        public Guid RoomTypeId { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual ICollection<AgreeableBreed> AgreeableBreeds { get; set; }
    }
}
