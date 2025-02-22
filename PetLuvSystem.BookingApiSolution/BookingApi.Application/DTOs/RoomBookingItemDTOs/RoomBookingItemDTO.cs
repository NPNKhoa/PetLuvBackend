namespace BookingApi.Application.DTOs.RoomBookingItemDTOs
{
    public record RoomBookingItemDTO
    (
        Guid BookingId,
        Guid RoomId,
        decimal ItemPrice
    );
}
