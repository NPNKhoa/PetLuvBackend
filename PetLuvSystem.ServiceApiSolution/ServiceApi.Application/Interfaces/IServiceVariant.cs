using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceVariant : IGenericInterface<IServiceVariant>
    {
        public Task<Response> GetByServiceAsync(Guid serviceId);
    }
}
