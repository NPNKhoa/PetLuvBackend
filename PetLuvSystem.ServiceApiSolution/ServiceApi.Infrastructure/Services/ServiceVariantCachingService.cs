using PetLuvSystem.SharedLibrary.Logs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using System.Text.Json;

namespace ServiceApi.Infrastructure.Services
{
    public class ServiceVariantCachingService(IRedisCacheService _cacheService) : IServiceVariantCachingService
    {
        private const string CacheKey = "checkPet";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCacheAsync(List<ServiceVariant> serviceVariants)
        {
            try
            {
                if (serviceVariants == null || !serviceVariants.Any())
                {
                    LogException.LogError("No pet found to update cache.");
                    return;
                }

                var visibleStatuses = serviceVariants
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
