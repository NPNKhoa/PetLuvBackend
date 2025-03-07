using RoomApi.Domain.Entities;

namespace RoomApi.Application.Interfaces
{
    public interface IRoomCachingService
    {
        public Task UpdateCacheAsync(List<Room> rooms);
    }
}
