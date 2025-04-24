using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceVariant : IGenericInterface<ServiceVariant>
    {
        public Task<Response> GetByKeyAsync(Guid serviceId, Guid breedId, string petWeightRange);
        public Task<Response> DeleteAsync(Guid serviceId, Guid breedId, string petWeightRange);
        public Task<Response> UpdateAsync(Guid serviceId, Guid breedId, string petWeightRange, ServiceVariant dto);
        public Task<Response> GetByServiceAsync(Guid serviceId);

        public Task<ServiceVariant> FindByKeyAsync(Guid serviceId, Guid breedId, string petWeightRange, bool noTracking);

    }
}
