using PetLuvSystem.SharedLibrary.Logs;
using RoomApi.Application.Interfaces;
using RoomApi.Domain.Entities;
using System.Text.Json;

namespace RoomApi.Infrastructure.Services
{
    public class RoomCachingService(IRedisCachingService _cacheService) : IRoomCachingService
    {
        private const string CacheKey = "getRoomById";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCacheAsync(List<Room> rooms)
        {
            try
            {
                if (rooms == null || !rooms.Any())
                {
                    LogException.LogError("No pet found to update cache.");
                    return;
                }

                var visibleStatuses = rooms
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
