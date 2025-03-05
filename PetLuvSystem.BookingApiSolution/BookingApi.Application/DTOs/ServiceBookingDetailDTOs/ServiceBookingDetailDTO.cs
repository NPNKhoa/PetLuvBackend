namespace BookingApi.Application.DTOs.ServiceBookingDetailDTOs
{
    public record ServiceBookingDetailDTO
    (
        Guid ServiceId,
        Guid BreedId,
        string PetWeightRange,
        Guid BookingId,
        string ServiceItemName,
        decimal BookingItemPrice
    );
}
