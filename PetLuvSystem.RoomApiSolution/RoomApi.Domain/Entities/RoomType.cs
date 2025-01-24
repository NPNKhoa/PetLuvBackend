using System.ComponentModel.DataAnnotations;

namespace RoomApi.Domain.Entities
{
    public class RoomType
    {
        [Required]
        public Guid RoomTypeId { get; set; }
        [Required]
        public string RoomTypeName { get; set; }
        [Required]
        public string RoomTypeDesc { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<RoomAccessory> RoomAccessories { get; set; }
        public virtual ICollection<AgreeableBreed> AgreeableBreeds { get; set; }
    }
}
