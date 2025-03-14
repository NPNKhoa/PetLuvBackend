using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceCombo : IGenericInterface<ServiceCombo>
    {
        public Task<ServiceCombo> FindByIdAsync(Guid id, bool noTracking, bool includeNavigation);
        public Task<Response> CreateAsync(ServiceCombo entity, ICollection<Guid> serviceIds);
        public Task<Response> UpdateAsync(Guid id, ServiceCombo entity, ICollection<Guid> serviceIds);
    }
}
