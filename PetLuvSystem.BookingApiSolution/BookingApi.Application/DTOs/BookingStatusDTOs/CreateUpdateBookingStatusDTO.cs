namespace BookingApi.Application.DTOs.BookingStatusDTOs
{
    public record CreateUpdateBookingStatusDTO
    (
        string BookingStatusName,
        bool IsVisible
    );
}
