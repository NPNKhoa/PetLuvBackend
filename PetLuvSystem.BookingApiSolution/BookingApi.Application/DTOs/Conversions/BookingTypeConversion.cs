using BookingApi.Application.DTOs.BookingTypeDTOs;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.DTOs.Conversions
{
    public static class BookingTypeConversion
    {
        public static BookingType ToEntity(BookingTypeDTO dto) => new()
        {
            BookingTypeId = dto.BookingTypeId,
            BookingTypeName = dto.BookingTypeName,
            BookingTypeDesc = dto.BookingTypeDesc,
            IsVisible = dto.IsVisible
        };

        public static BookingType ToEntity(CreateUpdateBookingTypeDTO dto) => new()
        {
            BookingTypeId = Guid.NewGuid(),
            BookingTypeName = dto.BookingTypeName,
            BookingTypeDesc = dto.BookingTypeDesc,
            IsVisible = dto.IsVisible
        };

        public static (BookingTypeDTO?, IEnumerable<BookingTypeDTO>?) FromEntity(BookingType? entity, IEnumerable<BookingType>? entities)
        {

            if (entity is not null && entities is null)
            {
                return (new BookingTypeDTO(
                    entity.BookingTypeId,
                    entity.BookingTypeName,
                    entity.BookingTypeDesc,
                    entity.IsVisible
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(
                    e => new BookingTypeDTO(
                    e.BookingTypeId,
                    e.BookingTypeName,
                    e.BookingTypeDesc,
                    e.IsVisible
                )
                ).ToList());
            }

            return (null, null);
        }
    }
}
