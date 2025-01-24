namespace RoomApi.Application.DTOs.RoomDTOs
{
    public record CreateUpdateRoomDTO
    (
        string RoomName,
        string RoomDesc,
        decimal PricePerHour,
        decimal PricePerDay,
        bool IsVisible,
        Guid RoomTypeId
    );
}
