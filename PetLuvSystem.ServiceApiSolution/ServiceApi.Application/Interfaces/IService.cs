using PetLuvSystem.SharedLibrary.Interfaces;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IService : IGenericInterface<Service>
    {
        public Task<Service> FindServiceById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
