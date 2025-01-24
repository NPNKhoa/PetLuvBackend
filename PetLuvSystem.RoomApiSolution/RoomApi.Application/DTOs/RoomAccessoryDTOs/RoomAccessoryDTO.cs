using System.ComponentModel.DataAnnotations;

namespace RoomApi.Application.DTOs.RoomAccessoryDTOs
{
    public record RoomAccessoryDTO
    (
        [Required]
        Guid RoomAccessoryId,
        [Required]
        string RoomAccessoryName,
        string RoomAccessoryDesc,
        string? RoomAccessoryImagePath,
        bool IsVisible,
        string RoomTypeName
    );
}
