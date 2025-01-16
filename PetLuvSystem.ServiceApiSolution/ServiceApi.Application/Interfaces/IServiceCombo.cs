using PetLuvSystem.SharedLibrary.Interfaces;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceCombo : IGenericInterface<ServiceCombo>
    {
        public Task<ServiceCombo> FindByIdAsync(Guid id, bool noTracking, bool includeNavigation);
    }
}
