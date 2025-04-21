using RoomApi.Application.DTOs.RoomTypeDTOs;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.DTOs.Conversions
{
    public static class RoomTypeConversion
    {
        public static RoomType ToEntity(RoomTypeDTO dto) => new()
        {
            RoomTypeId = dto.RoomTypeId,
            RoomTypeName = dto.RoomTypeName,
            RoomTypeDesc = dto.RoomTypeDesc,
            IsVisible = dto.IsVisible,
            Rooms = dto.Rooms,
            RoomAccessories = dto.RoomAccessories,
        };

        public static RoomType ToEntity(CreateUpdateRoomTypeDTO dto) => new()
        {
            RoomTypeId = Guid.Empty,
            RoomTypeName = dto.RoomTypeName,
            RoomTypeDesc = dto.RoomTypeDesc,
            IsVisible = dto.IsVisible,
            Rooms = new List<Room>(),
            RoomAccessories = new List<RoomAccessory>(),
        };

        public static (RoomTypeDTO?, IEnumerable<RoomTypeDTO>?) FromEntity(RoomType? entity, IEnumerable<RoomType>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDto = new RoomTypeDTO(
                    entity.RoomTypeId,
                    entity.RoomTypeName,
                    entity.RoomTypeDesc,
                    entity.IsVisible,
                    entity.Rooms,
                    entity.RoomAccessories
                );

                return (singleDto, null);
            }

            if (entities is not null && entity is null)
            {
                var listDto = entities.Select(e => new RoomTypeDTO(
                    e.RoomTypeId,
                    e.RoomTypeName,
                    e.RoomTypeDesc,
                    e.IsVisible,
                    e.Rooms,
                    e.RoomAccessories
                ));

                return (null, listDto);
            }

            return (null, null);
        }
    }
}
