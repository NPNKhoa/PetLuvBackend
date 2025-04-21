using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Domain.Entities;

namespace RoomApi.Application.Interfaces
{
    public interface IRoom : IGenericInterface<Room>
    {
        public Task<Room> FindById(Guid id, bool noTracking = false, bool includeImages = false, bool includeType = false);
        public Task<Response> GetValidRoom(IEnumerable<Guid> breedIds);
    }
}
