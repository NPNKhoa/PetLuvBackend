using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IFoodVariant : IGenericInterface<FoodVariant>
    {
        public Task<FoodVariant> FindById(Guid FoodId, Guid FlavorId, Guid SizeId, bool noTracking = false, bool includeOthers = false);
        public Task<Response> GetByKey(Guid FoodId, Guid FlavorId, Guid SizeId);
        public Task<Response> Delete(Guid FoodId, Guid FlavorId, Guid SizeId);
        public Task<Response> Update(Guid FoodId, Guid FlavorId, Guid SizeId, FoodVariant foodVariant);
    }
}
