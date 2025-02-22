namespace BookingApi.Application.DTOs.BookingTypeDTOs
{
    public record CreateUpdateBookingTypeDTO
    (
        string BookingTypeName,
        string BookingTypeDesc,
        bool IsVisible
    );
}
