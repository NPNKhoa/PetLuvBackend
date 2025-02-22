namespace BookingApi.Application.DTOs.BookingStatusDTOs
{
    public record BookingStatusDTO
    (
        Guid BookingStatusId,
        string BookingStatusName,
        bool IsVisible
    );
}
