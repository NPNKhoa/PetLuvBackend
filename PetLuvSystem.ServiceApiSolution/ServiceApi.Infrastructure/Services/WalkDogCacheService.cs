using PetLuvSystem.SharedLibrary.Logs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using System.Text.Json;

namespace ServiceApi.Infrastructure.Services
{
    public class WalkDogCacheService(IRedisCacheService _cacheService) : IWalkDogCacheService
    {
        private const string CacheKey = "walkDogService";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCacheAsync(List<WalkDogServiceVariant> variants)
        {
            try
            {
                if (variants == null || !variants.Any())
                {
                    LogException.LogError("No pet found to update cache.");
                    return;
                }

                var visibleStatuses = variants
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
