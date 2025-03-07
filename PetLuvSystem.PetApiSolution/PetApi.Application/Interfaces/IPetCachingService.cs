using PetApi.Domain.Entities;

namespace PetApi.Application.Interfaces
{
    public interface IPetCachingService
    {
        public Task UpdateCache(List<Pet> pets);
    }
}
