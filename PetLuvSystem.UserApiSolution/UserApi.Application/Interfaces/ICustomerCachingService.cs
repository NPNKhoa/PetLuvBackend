using UserApi.Domain.Etities;

namespace UserApi.Application.Interfaces
{
    public interface ICustomerCachingService
    {
        public Task UpdateCache(List<User> users);
    }
}
