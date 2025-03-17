using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;

namespace PetApi.Infrastructure.Services
{
    public class BreedMappingCacheUpdateService : IBreedMappingCacheUpdateService
    {
        private readonly IRedisCacheService _cacheService;
        private const string CacheKey = "breedMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public BreedMappingCacheUpdateService(IRedisCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task UpdateBreedMappingCacheAsync(PetBreed petBreed)
        {
            if (petBreed == null)
            {
                LogException.LogError("Attempted to update breed mapping cache with a null PetBreed entity.");
                return;
            }

            await UpdateBreedMappingCacheAsync(new List<PetBreed> { petBreed });
        }

        public async Task UpdateBreedMappingCacheAsync(IEnumerable<PetBreed> petBreeds)
        {
            if (petBreeds == null || !petBreeds.Any())
            {
                LogException.LogError("Attempted to update breed mapping cache with empty data.");
                return;
            }

            try
            {
                var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);
                var cacheDict = !string.IsNullOrEmpty(cachedData)
                    ? JsonSerializer.Deserialize<Dictionary<string, string>>(cachedData)
                    : new Dictionary<string, string>();

                if (cacheDict == null)
                {
                    cacheDict = new Dictionary<string, string>();
                }

                foreach (var petBreed in petBreeds)
                {
                    cacheDict[petBreed.BreedId.ToString()] = petBreed.BreedName;
                }

                var serializedData = JsonSerializer.Serialize(cacheDict);
                await _cacheService.SetCachedValueAsync(CacheKey, serializedData, CacheExpiry);
                LogException.LogInformation("Breed mapping cache successfully updated.");
            }
            catch (Exception ex)
            {
                LogException.LogError("Failed to update breed mapping cache.");
                LogException.LogExceptions(ex);
            }
        }
    }

}
