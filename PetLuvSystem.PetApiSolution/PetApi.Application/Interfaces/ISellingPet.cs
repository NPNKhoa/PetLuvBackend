using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PetApi.Application.Interfaces
{
    public interface ISellingPet : IGenericInterface<SellingPet>
    {
        public Task<SellingPet> FindById(Guid id, bool noTracking = false, bool includeRelatedData = false);
    }
}
