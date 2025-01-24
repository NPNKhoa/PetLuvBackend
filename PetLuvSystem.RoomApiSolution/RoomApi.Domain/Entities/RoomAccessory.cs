using System.ComponentModel.DataAnnotations;

namespace RoomApi.Domain.Entities
{
    public class RoomAccessory
    {
        [Required]
        public Guid RoomAccessoryId { get; set; }
        [Required]
        public string RoomAccessoryName { get; set; }
        [Required]
        public string RoomAccessoryDesc { get; set; }
        public string? RoomAccessoryImagePath { get; set; }
        public bool IsVisible { get; set; }

        public Guid RoomTypeId { get; set; }
        public virtual RoomType RoomType { get; set; }
        public Guid ServiceId { get; set; }
    }
}
