using PetLuvSystem.SharedLibrary.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IFoodSize : IGenericInterface<FoodSize>
    {
        public Task<FoodSize> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
