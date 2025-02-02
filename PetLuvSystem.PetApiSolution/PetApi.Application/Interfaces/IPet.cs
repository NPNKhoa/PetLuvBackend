using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PetApi.Application.Interfaces
{
    public interface IPet : IGenericInterface<Pet>
    {
        public Task<Pet> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
