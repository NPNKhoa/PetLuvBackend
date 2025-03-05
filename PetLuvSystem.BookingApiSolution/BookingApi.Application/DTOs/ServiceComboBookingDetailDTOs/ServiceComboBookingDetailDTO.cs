namespace BookingApi.Application.DTOs.ServiceComboBookingDetailDTOs
{
    public record ServiceComboBookingDetailDTO
    (
        Guid ServiceComboId,
        Guid BreedId,
        string PetWeightRange,
        Guid BookingId,
        string ServiceComboItemName,
        decimal BookingItemPrice
    );
}
