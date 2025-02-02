using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PetApi.Application.Interfaces
{
    public interface IPetBreed : IGenericInterface<PetBreed>
    {
        public Task<PetBreed> FindById(Guid id, bool noTracking = false, bool include = false);
    }
}
