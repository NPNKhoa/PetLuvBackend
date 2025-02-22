using BookingApi.Application.DTOs.BookingStatusDTOs;
using BookingApi.Application.DTOs.BookingTypeDTOs;
using BookingApi.Application.DTOs.RoomBookingItemDTOs;
using BookingApi.Application.DTOs.ServiceBookingDetailDTOs;
using BookingApi.Application.DTOs.ServiceComboBookingDetailDTOs;

namespace BookingApi.Application.DTOs.BookingDTOs
{
    public record BookingDTO
    (
        Guid BookingId,
        DateTime BookingStartTime,
        DateTime BookingEndTime,
        string? BookingNote,
        decimal TotalAmount,
        decimal DepositAmount,
        string TotalEstimateTime,
        Guid BookingTypeId,
        BookingTypeDTO BookingType,
        Guid BookingStatusId,
        BookingStatusDTO BookingStatus,
        Guid PaymentStatusId,
        Guid CustomerId,
        Guid PetId,
        RoomBookingItemDTO RoomBookingItem,
        ServiceBookingDetailDTO ServiceBookingDetail,
        ServiceComboBookingDetailDTO ServiceComboBookingDetail
    );
}
