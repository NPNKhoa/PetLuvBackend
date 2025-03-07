using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceVariantCachingService
    {
        public Task UpdateCacheAsync(List<ServiceVariant> serviceVariants);
    }
}
