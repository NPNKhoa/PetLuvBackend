using PetLuvSystem.SharedLibrary.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IFoodFlavor : IGenericInterface<FoodFlavor>
    {
        public Task<FoodFlavor> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
