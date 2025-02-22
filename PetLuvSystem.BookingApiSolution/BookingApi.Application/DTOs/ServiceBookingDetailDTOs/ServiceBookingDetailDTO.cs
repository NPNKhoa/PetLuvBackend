namespace BookingApi.Application.DTOs.ServiceBookingDetailDTOs
{
    public record ServiceBookingDetailDTO
    (
        Guid ServiceVariantId,
        Guid BookingId,
        string ServiceItemName,
        decimal BookingItemPrice
    );
}
