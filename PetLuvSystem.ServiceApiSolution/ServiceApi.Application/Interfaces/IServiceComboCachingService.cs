using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceComboCachingService
    {
        public Task Updatecache(List<ServiceComboVariant> serviceComboVariants);
    }
}
