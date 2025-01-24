using System.ComponentModel.DataAnnotations;

namespace RoomApi.Application.DTOs.RoomAccessoryDTOs
{
    public record CreateUpdateRoomAccessoryDTO
    (
        [Required]
        string RoomAccessoryName,
        string RoomAccessoryDesc,
        bool IsVisible,
        Guid RoomtypeId
    );
}
