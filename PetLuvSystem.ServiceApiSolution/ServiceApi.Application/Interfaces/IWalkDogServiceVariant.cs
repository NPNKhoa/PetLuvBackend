using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IWalkDogServiceVariant : IGenericInterface<WalkDogServiceVariant>
    {
        public Task<Response> GetByKeyAsync(Guid serviceId, Guid breedId);
        public Task<Response> DeleteAsync(Guid serviceId, Guid breedId);
        public Task<Response> UpdateAsync(Guid serviceId, Guid breedId, decimal pricePerPeriod);
        public Task<Response> GetByServiceAsync(Guid serviceId);

        public Task<WalkDogServiceVariant> FindByKeyAsync(Guid serviceId, Guid breedId, bool noTracking);
    }
}
