using PetLuvSystem.SharedLibrary.Interfaces;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.Interfaces
{
    public interface IRoomAccessory : IGenericInterface<RoomAccessory>
    {
        public Task<RoomAccessory> FindById(Guid id, bool noTracking = false, bool includeRoomType = false);
    }
}
