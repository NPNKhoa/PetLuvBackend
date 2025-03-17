using PetApi.Domain.Entities;

namespace PetApi.Application.Interfaces
{
    public interface IBreedMappingCacheUpdateService
    {
        /// <summary>
        /// Updates the breed mapping cache with a single PetBreed entity.
        /// </summary>
        Task UpdateBreedMappingCacheAsync(PetBreed petBreed);

        /// <summary>
        /// Updates the breed mapping cache with multiple PetBreed entities.
        /// </summary>
        Task UpdateBreedMappingCacheAsync(IEnumerable<PetBreed> petBreeds);
    }

}
