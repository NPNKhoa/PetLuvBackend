namespace BookingApi.Application.DTOs.BookingTypeDTOs
{
    public record BookingTypeDTO
    (
        Guid BookingTypeId,
        string BookingTypeName,
        string BookingTypeDesc,
        bool IsVisible
    );
}
