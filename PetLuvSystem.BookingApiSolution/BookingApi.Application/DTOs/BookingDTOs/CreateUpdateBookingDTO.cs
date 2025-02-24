namespace BookingApi.Application.DTOs.BookingDTOs
{
    public record CreateUpdateBookingDTO
    (
        DateTime BookingStartTime,
        DateTime BookingEndTime,
        string? BookingNote,
        decimal TotalAmount,
        decimal DepositAmount,
        string TotalEstimateTime,
        Guid BookingTypeId,
        Guid BookingStatusId,
        Guid PaymentStatusId,
        Guid CustomerId,
        Guid PetId,
        Guid? RoomId,
        IEnumerable<Guid>? ServiceId,
        IEnumerable<Guid>? ServiceComboIds
    );
}
