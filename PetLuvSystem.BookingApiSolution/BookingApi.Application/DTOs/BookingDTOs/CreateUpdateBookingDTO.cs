namespace BookingApi.Application.DTOs.BookingDTOs
{
    public record CreateUpdateBookingDTO
    (
        DateTime BookingStartTime,
        DateTime BookingEndTime,
        string? BookingNote,
        int? RoomRentalTime,
        Guid BookingTypeId,
        Guid CustomerId,
        string CustomerEmail,
        Guid PetId,
        Guid? BreedId,
        string? PetWeightRange,
        Guid? RoomId,
        IEnumerable<Guid>? ServiceId,
        IEnumerable<string>? ServiceNames,
        IEnumerable<Guid>? ServiceComboIds
    );
}
