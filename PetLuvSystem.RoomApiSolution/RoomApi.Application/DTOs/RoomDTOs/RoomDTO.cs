namespace RoomApi.Application.DTOs.RoomDTOs
{
    public record RoomDTO
    (
        Guid RoomId,
        string RoomName,
        string RoomDesc,
        decimal PricePerHour,
        decimal PricePerDay,
        bool IsVisible,
        Guid RoomTypeId,
        string RoomTypeName,

        ICollection<string> RoomImages
    );
}
