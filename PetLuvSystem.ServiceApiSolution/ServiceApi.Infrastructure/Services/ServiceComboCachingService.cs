using PetLuvSystem.SharedLibrary.Logs;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using System.Text.Json;

namespace ServiceApi.Infrastructure.Services
{
    class ServiceComboCachingService(IRedisCacheService _cacheService) : IServiceComboCachingService
    {
        private const string CacheKey = "ComboService";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task Updatecache(List<ServiceComboVariant> serviceComboVariants)
        {
            try
            {
                if (serviceComboVariants == null || !serviceComboVariants.Any())
                {
                    LogException.LogError("No combo found to update cache.");
                    return;
                }

                var visibleStatuses = serviceComboVariants
                    .Where(p => p.IsVisible)
                    .ToHashSet();

                var jsonData = JsonSerializer.Serialize(visibleStatuses);

                await _cacheService.SetCachedValueAsync(CacheKey, jsonData, CacheExpiry);

                LogException.LogInformation("combo cache updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
