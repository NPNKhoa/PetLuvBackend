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
        int? RoomRentalTime,
        Guid BookingTypeId,
        BookingTypeDTO BookingType,
        Guid BookingStatusId,
        BookingStatusDTO BookingStatus,
        Guid PaymentStatusId,
        string PaymentStatusName,
        Guid CustomerId,
        Guid PetId,
        RoomBookingItemDTO RoomBookingItem,
        ICollection<ServiceBookingDetailDTO> ServiceBookingDetails,
        ICollection<ServiceComboBookingDetailDTO> ServiceComboBookingDetails
    );
}
