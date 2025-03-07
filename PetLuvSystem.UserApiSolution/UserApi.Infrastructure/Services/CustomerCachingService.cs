using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using UserApi.Application.Interfaces;
using UserApi.Domain.Etities;

namespace UserApi.Infrastructure.Services
{
    public class CustomerCachingService(IRedisCacheService _cacheService) : ICustomerCachingService
    {
        private const string CacheKey = "checkCustomer";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCache(List<User> users)
        {
            try
            {
                if (users == null || !users.Any())
                {
                    LogException.LogError("No user found to update cache.");
                    return;
                }

                // Lọc chỉ những trạng thái "visible"
                var visibleStatuses = users
                    .Where(p => p.IsActive)
                    .ToHashSet();

                // Chuyển đổi sang JSON
                var jsonData = JsonSerializer.Serialize(visibleStatuses);

                // Cập nhật cache
                await _cacheService.SetCachedValueAsync(CacheKey, jsonData, CacheExpiry);

                LogException.LogInformation("Customer cache updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
