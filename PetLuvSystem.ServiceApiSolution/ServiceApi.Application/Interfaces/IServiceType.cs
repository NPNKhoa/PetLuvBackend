using PetLuvSystem.SharedLibrary.Interfaces;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceType : IGenericInterface<ServiceType>
    {
        public Task<ServiceType> FindByIdAsync(Guid id);
    }
}
