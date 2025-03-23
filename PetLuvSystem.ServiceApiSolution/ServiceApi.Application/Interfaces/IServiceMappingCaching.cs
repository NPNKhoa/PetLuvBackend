using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceMappingCaching
    {
        public Task UpdateCacheAsync(List<Service> services);
    }
}
