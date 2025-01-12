using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceImage : IGenericInterface<ServiceImage>
    {
        public Task<ServiceImage> FindByPathAsync(string serviceImagePath);
        public Task<Response> DeleteAsync(string serviceImagePath);
        public Task<Response> GetServiceImageByServiceId(Guid serviceId);
    }
}
