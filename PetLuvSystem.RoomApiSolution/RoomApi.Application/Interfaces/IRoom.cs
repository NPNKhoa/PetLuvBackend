using PetLuvSystem.SharedLibrary.Interfaces;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.Interfaces
{
    public interface IRoom : IGenericInterface<Room>
    {
        public Task<Room> FindById(Guid id, bool noTracking = false, bool includeImages = false, bool includeType = false);
    }
}
