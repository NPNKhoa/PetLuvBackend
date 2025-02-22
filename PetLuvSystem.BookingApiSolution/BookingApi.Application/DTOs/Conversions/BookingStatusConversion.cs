using BookingApi.Application.DTOs.BookingStatusDTOs;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.DTOs.Conversions
{
    public static class BookingStatusConversion
    {
        public static BookingStatus ToEntity(BookingStatusDTO dto) => new()
        {
            BookingStatusId = dto.BookingStatusId,
            BookingStatusName = dto.BookingStatusName,
            IsVisible = dto.IsVisible
        };

        public static BookingStatus ToEntity(CreateUpdateBookingStatusDTO dto) => new()
        {
            BookingStatusId = Guid.NewGuid(),
            BookingStatusName = dto.BookingStatusName,
            IsVisible = dto.IsVisible
        };

        public static (BookingStatusDTO?, IEnumerable<BookingStatusDTO>?) FromEntity(BookingStatus? entity, IEnumerable<BookingStatus>? entities)
        {

            if (entity is not null && entities is null)
            {
                return (new BookingStatusDTO(
                    entity.BookingStatusId,
                    entity.BookingStatusName,
                    entity.IsVisible
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(
                    e => new BookingStatusDTO(
                        e.BookingStatusId,
                        e.BookingStatusName,
                        e.IsVisible
                    )
                ).ToList());
            }

            return (null, null);
        }
    }
}
