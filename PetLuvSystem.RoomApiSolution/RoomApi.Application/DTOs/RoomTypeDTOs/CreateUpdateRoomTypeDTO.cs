using System.ComponentModel.DataAnnotations;

namespace RoomApi.Application.DTOs.RoomTypeDTOs
{
    public record CreateUpdateRoomTypeDTO
    (
        [Required, MaxLength(50)]
        string RoomTypeName,
        [MaxLength(500)]
        string RoomTypeDesc,
        bool IsVisible
    );
}
