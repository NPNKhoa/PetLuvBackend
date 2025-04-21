using RoomApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RoomApi.Application.DTOs.RoomTypeDTOs
{
    public record RoomTypeDTO
    (
        [Required]
        Guid RoomTypeId,
        [Required, MaxLength(50)]
        string RoomTypeName,
        [MaxLength(500)]
        string RoomTypeDesc,
        bool IsVisible,
        ICollection<Room> Rooms,
        ICollection<RoomAccessory> RoomAccessories
    );
}
