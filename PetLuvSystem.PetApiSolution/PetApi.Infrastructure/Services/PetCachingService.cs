using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;

namespace PetApi.Infrastructure.Services
{
    public class PetCachingService(IRedisCacheService _cacheService) : IPetCachingService
    {
        private const string CacheKey = "checkPet";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCache(List<Pet> pets)
        {
            try
            {
                if (pets == null || !pets.Any())
                {
                    LogException.LogError("No pet found to update cache.");
                    return;
                }

                var visibleStatuses = pets
                    .Where(p => p.IsVisible)
                    .ToHashSet();

                var jsonData = JsonSerializer.Serialize(visibleStatuses);

                await _cacheService.SetCachedValueAsync(CacheKey, jsonData, CacheExpiry);

                LogException.LogInformation("Pet cache updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
