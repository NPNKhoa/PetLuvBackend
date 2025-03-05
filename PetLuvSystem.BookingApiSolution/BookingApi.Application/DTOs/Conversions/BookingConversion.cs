using BookingApi.Application.DTOs.BookingDTOs;
using BookingApi.Application.DTOs.BookingStatusDTOs;
using BookingApi.Application.DTOs.BookingTypeDTOs;
using BookingApi.Application.DTOs.RoomBookingItemDTOs;
using BookingApi.Application.DTOs.ServiceBookingDetailDTOs;
using BookingApi.Application.DTOs.ServiceComboBookingDetailDTOs;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.DTOs.Conversions
{
    public static class BookingConversion
    {
        public static Booking ToEntity(BookingDTO dto) => new()
        {
            BookingId = dto.BookingId,
            BookingStartTime = dto.BookingStartTime,
            BookingEndTime = dto.BookingEndTime,
            BookingNote = dto.BookingNote,
            TotalAmount = dto.TotalAmount,
            DepositAmount = dto.DepositAmount,
            TotalEstimateTime = dto.TotalEstimateTime,
            BookingTypeId = dto.BookingTypeId,
            BookingType = new BookingType()
            {
                BookingTypeId = dto.BookingType.BookingTypeId,
                BookingTypeName = dto.BookingType.BookingTypeName,
                BookingTypeDesc = dto.BookingType.BookingTypeDesc,
                IsVisible = dto.BookingType.IsVisible
            },
            BookingStatusId = dto.BookingStatusId,
            BookingStatus = new BookingStatus()
            {
                BookingStatusId = dto.BookingStatus.BookingStatusId,
                BookingStatusName = dto.BookingStatus.BookingStatusName,
                IsVisible = dto.BookingStatus.IsVisible
            },
            PaymentStatusId = dto.PaymentStatusId,
            CustomerId = dto.CustomerId,
            PetId = dto.PetId,
            RoomBookingItem = new RoomBookingItem()
            {
                BookingId = dto.BookingId,
                RoomId = dto.RoomBookingItem.RoomId,
                ItemPrice = dto.RoomBookingItem.ItemPrice
            },
            ServiceBookingDetails = dto.ServiceBookingDetails.Select(s => new ServiceBookingDetail()
            {
                ServiceId = s.ServiceId,
                BreedId = s.BreedId,
                PetWeightRange = s.PetWeightRange,
                BookingId = s.BookingId,
                ServiceItemName = s.ServiceItemName,
                BookingItemPrice = s.BookingItemPrice
            }).ToList(),
            ServiceComboBookingDetails = dto.ServiceComboBookingDetails.Select(s => new ServiceComboBookingDetail()
            {
                ServiceComboId = s.ServiceComboId,
                BreedId = s.BreedId,
                PetWeightRange = s.PetWeightRange,
                BookingId = s.BookingId,
                ServiceComboItemName = s.ServiceComboItemName,
                BookingItemPrice = s.BookingItemPrice
            }).ToList()
        };

        public static Booking ToEntity(CreateUpdateBookingDTO dto) => new()
        {
            BookingId = Guid.NewGuid(),
            BookingStartTime = dto.BookingStartTime,
            BookingEndTime = dto.BookingEndTime,
            BookingNote = dto.BookingNote,
            BookingTypeId = dto.BookingTypeId,
            CustomerId = dto.CustomerId,
            PetId = dto.PetId
        };

        public static (BookingDTO?, IEnumerable<BookingDTO>?) FromEntity(Booking? entity, IEnumerable<Booking>? entities)
        {

            if (entity is not null && entities is null)
            {
                return (new BookingDTO(
                        entity.BookingId,
                        entity.BookingStartTime,
                        entity.BookingEndTime,
                        entity.BookingNote ?? string.Empty,
                        entity.TotalAmount,
                        entity.DepositAmount,
                        entity.TotalEstimateTime,
                        entity.RoomRentalTime,
                        entity.BookingTypeId,
                        entity.BookingType is not null ? new BookingTypeDTO(
                            entity.BookingType.BookingTypeId,
                            entity.BookingType.BookingTypeName,
                            entity.BookingType.BookingTypeDesc,
                            entity.BookingType.IsVisible
                        ) : null!,
                        entity.BookingStatusId,
                        entity.BookingStatus is not null ? new BookingStatusDTO(
                            entity.BookingStatus.BookingStatusId,
                            entity.BookingStatus.BookingStatusName,
                            entity.BookingStatus.IsVisible
                        ) : null!,
                        entity.PaymentStatusId,
                        entity.CustomerId,
                        entity.PetId,
                        entity.RoomBookingItem is not null ? new RoomBookingItemDTO(
                            entity.RoomBookingItem.BookingId,
                            entity.RoomBookingItem.RoomId,
                            entity.RoomBookingItem.ItemPrice
                        ) : null!,
                        entity.ServiceBookingDetails is not null
                            && entity.ServiceBookingDetails.Any()
                                ? entity.ServiceBookingDetails.Select(s => new ServiceBookingDetailDTO(
                            s.ServiceId,
                            s.BreedId,
                            s.PetWeightRange,
                            s.BookingId,
                            s.ServiceItemName,
                            s.BookingItemPrice
                        )).ToList() : null!,
                        entity.ServiceComboBookingDetails is not null
                            && entity.ServiceComboBookingDetails.Any()
                            ? entity.ServiceComboBookingDetails.Select(s => new ServiceComboBookingDetailDTO(
                            s.ServiceComboId,
                            s.BreedId,
                            s.PetWeightRange,
                            s.BookingId,
                            s.ServiceComboItemName,
                            s.BookingItemPrice
                        )).ToList() : null!
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(e => new BookingDTO(
                    e.BookingId,
                    e.BookingStartTime,
                    e.BookingEndTime,
                    e.BookingNote ?? string.Empty,
                    e.TotalAmount,
                    e.DepositAmount,
                    e.TotalEstimateTime,
                    e.RoomRentalTime,
                    e.BookingTypeId,
                    e.BookingType is not null ? new BookingTypeDTO(
                        e.BookingType.BookingTypeId,
                        e.BookingType.BookingTypeName,
                        e.BookingType.BookingTypeDesc,
                        e.BookingType.IsVisible
                    ) : null!,
                    e.BookingStatusId,
                    e.BookingStatus is not null ? new BookingStatusDTO(
                        e.BookingStatus.BookingStatusId,
                        e.BookingStatus.BookingStatusName,
                        e.BookingStatus.IsVisible
                    ) : null!,
                    e.PaymentStatusId,
                    e.CustomerId,
                    e.PetId,
                    e.RoomBookingItem is not null ? new RoomBookingItemDTO(
                        e.RoomBookingItem.BookingId,
                        e.RoomBookingItem.RoomId,
                        e.RoomBookingItem.ItemPrice
                    ) : null!,
                    e.ServiceBookingDetails is not null
                        && e.ServiceBookingDetails.Any()
                            ? e.ServiceBookingDetails.Select(s => new ServiceBookingDetailDTO(
                        s.ServiceId,
                        s.BreedId,
                        s.PetWeightRange,
                        s.BookingId,
                        s.ServiceItemName,
                        s.BookingItemPrice
                    )).ToList() : null!,
                    e.ServiceComboBookingDetails is not null
                        && e.ServiceComboBookingDetails.Any()
                        ? e.ServiceComboBookingDetails.Select(s => new ServiceComboBookingDetailDTO(
                        s.ServiceComboId,
                        s.BreedId,
                        s.PetWeightRange,
                        s.BookingId,
                        s.ServiceComboItemName,
                        s.BookingItemPrice
                    )).ToList() : null!
                )));
            }

            return (null, null);
        }
    }
}
