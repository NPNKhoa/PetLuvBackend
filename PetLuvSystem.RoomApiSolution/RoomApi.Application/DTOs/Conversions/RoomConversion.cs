using RoomApi.Application.DTOs.RoomDTOs;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.DTOs.Conversions
{
    public static class RoomConversion
    {
        public static Room ToEntity(RoomDTO dto) => new()
        {
            RoomId = dto.RoomId,
            RoomName = dto.RoomName,
            RoomDesc = dto.RoomDesc,
            PricePerHour = dto.PricePerHour,
            PricePerDay = dto.PricePerDay,
            IsVisible = dto.IsVisible,
            RoomImages = dto.RoomImages.Select(path => new RoomImage()
            {
                RoomImagePath = path,
                RoomId = dto.RoomId
            }).ToList()
        };

        public static Room ToEntity(CreateUpdateRoomDTO dto) => new()
        {
            RoomId = Guid.Empty,
            RoomName = dto.RoomName,
            RoomDesc = dto.RoomDesc,
            PricePerHour = dto.PricePerHour,
            PricePerDay = dto.PricePerDay,
            IsVisible = dto.IsVisible,
        };

        public static (RoomDTO?, IEnumerable<RoomDTO>?) FromEntity(Room? entity, IEnumerable<Room>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDto = new RoomDTO(
                    entity.RoomId,
                    entity.RoomName,
                    entity.RoomDesc,
                    entity.PricePerHour,
                    entity.PricePerDay,
                    entity.IsVisible,
                    entity.RoomType.RoomTypeName,
                    entity.RoomImages.Select(ri => ri.RoomImagePath).ToList()
                );

                return (singleDto, null);
            }

            if (entities is not null && entity is null)
            {
                var listDto = entities.Select(e => new RoomDTO(
                    e.RoomId,
                    e.RoomName,
                    e.RoomDesc,
                    e.PricePerHour,
                    e.PricePerDay,
                    e.IsVisible,
                    e.RoomType.RoomTypeName,
                    e.RoomImages.Select(ri => ri.RoomImagePath).ToList()
                ));

                return (null, listDto);
            }

            return (null, null);
        }
    }
}
