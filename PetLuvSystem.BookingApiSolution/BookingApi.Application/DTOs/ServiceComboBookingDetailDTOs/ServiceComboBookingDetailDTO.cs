namespace BookingApi.Application.DTOs.ServiceComboBookingDetailDTOs
{
    public record ServiceComboBookingDetailDTO
    (
        Guid ServiceComboId,
        Guid BookingId,
        string ServiceComboItemName,
        decimal BookingItemPrice
    );
}
