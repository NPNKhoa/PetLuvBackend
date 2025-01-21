using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceComboMapping : IGenericInterface<ServiceComboMapping>
    {
        public Task<ServiceComboMapping> FindByKeyAsync(Guid serviceId, Guid serviceComboId, bool noTracking);
        public Task<Response> DeleteAsync(Guid serviceId, Guid serviceComboId);
        public Task<Response> GetServicesByCombo(Guid serviceComboId);
    }
}
