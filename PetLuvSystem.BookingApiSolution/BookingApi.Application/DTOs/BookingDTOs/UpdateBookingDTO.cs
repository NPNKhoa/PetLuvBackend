namespace BookingApi.Application.DTOs.BookingDTOs
{

    public record UpdateBookingDTO
    (
        DateTime BookingStartTime,
        DateTime BookingEndTime,
        int? RoomRentalTime,
        string? TotalEstimateTime,
        Guid PaymentStatusId,
        Guid BookingStatusId
    );
}
