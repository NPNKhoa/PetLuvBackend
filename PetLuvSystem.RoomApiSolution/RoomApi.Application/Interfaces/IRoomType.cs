using PetLuvSystem.SharedLibrary.Interfaces;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.Interfaces
{
    public interface IRoomType : IGenericInterface<RoomType>
    {
        public Task<RoomType> FindById(Guid id, bool noTracking = false, bool includeRoom = false);
    }
}
