using PetLuvSystem.SharedLibrary.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IFood : IGenericInterface<Food>
    {
        public Task<Food> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
