using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PetApi.Application.Interfaces
{
    public interface IPetType : IGenericInterface<PetType>
    {
        public Task<PetType> FindById(Guid id, bool noTracking = false, bool includeBreed = false);
    }
}
