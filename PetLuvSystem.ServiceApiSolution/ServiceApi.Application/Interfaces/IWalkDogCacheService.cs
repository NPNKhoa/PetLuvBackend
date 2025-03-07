using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IWalkDogCacheService
    {
        public Task UpdateCacheAsync(List<WalkDogServiceVariant> variants);
    }
}
