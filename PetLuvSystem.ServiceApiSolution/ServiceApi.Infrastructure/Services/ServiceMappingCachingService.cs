using PetLuvSystem.SharedLibrary.Logs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using System.Text.Json;

namespace ServiceApi.Infrastructure.Services
{
    public class ServiceMappingCachingService(IRedisCacheService _cacheService) : IServiceMappingCaching
    {
        private const string CacheKey = "ServiceMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCacheAsync(List<Service> services)
        {
            try
            {
                if (services == null || !services.Any())
                {
                    LogException.LogError("No service found to update cache.");
                    return;
                }

                var visibleStatuses = services
                    .Where(p => p.IsVisible)
                    .ToHashSet();

                var jsonData = JsonSerializer.Serialize(visibleStatuses);

                await _cacheService.SetCachedValueAsync(CacheKey, jsonData, CacheExpiry);

                LogException.LogInformation("service cache updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
