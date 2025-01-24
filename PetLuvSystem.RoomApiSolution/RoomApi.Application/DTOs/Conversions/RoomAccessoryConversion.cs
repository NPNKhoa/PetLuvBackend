using RoomApi.Application.DTOs.RoomAccessoryDTOs;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.DTOs.Conversions
{
    public static class RoomAccessoryConversion
    {
        public static RoomAccessory ToEntity(RoomAccessoryDTO dto) => new()
        {
            RoomAccessoryId = dto.RoomAccessoryId,
            RoomAccessoryName = dto.RoomAccessoryName,
            RoomAccessoryDesc = dto.RoomAccessoryDesc,
            RoomAccessoryImagePath = dto.RoomAccessoryImagePath,
            IsVisible = dto.IsVisible,
            RoomTypeId = Guid.Empty,
        };

        public static RoomAccessory ToEntity(CreateUpdateRoomAccessoryDTO dto) => new()
        {
            RoomAccessoryId = Guid.Empty,
            RoomAccessoryName = dto.RoomAccessoryName,
            RoomAccessoryDesc = dto.RoomAccessoryDesc,
            IsVisible = dto.IsVisible,
            RoomTypeId = dto.RoomtypeId,
        };

        public static (RoomAccessoryDTO?, IEnumerable<RoomAccessoryDTO>?) FromEntity(RoomAccessory? entity, IEnumerable<RoomAccessory>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDto = new RoomAccessoryDTO(
                    entity.RoomAccessoryId,
                    entity.RoomAccessoryName,
                    entity.RoomAccessoryDesc,
                    entity?.RoomAccessoryImagePath,
                    entity!.IsVisible,
                    entity.RoomType.RoomTypeName
                );

                return (singleDto, null);
            }

            if (entities is not null && entity is null)
            {
                var listDto = entities.Select(e => new RoomAccessoryDTO(
                    e.RoomAccessoryId,
                    e.RoomAccessoryName,
                    e.RoomAccessoryDesc,
                    e?.RoomAccessoryImagePath,
                    e!.IsVisible,
                    e.RoomType.RoomTypeName
                ));

                return (null, listDto);
            }

            return (null, null);
        }
    }
}
