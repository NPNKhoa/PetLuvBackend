using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;

namespace PaymentApi.Infrastructure.Services
{
    public class PaymentCachingService(IRedisCacheService _cacheService) : IPaymentCachingService
    {
        private const string CacheKey = "checkPaymentStatus";
        private const string MappingCacheKey = "getPaymentStatusMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public async Task UpdateCacheAsync(List<PaymentStatus> paymentStatuses)
        {
            try
            {
                if (paymentStatuses == null || !paymentStatuses.Any())
                {
                    LogException.LogError("No payment status found to update cache.");
                    return;
                }

                // Lọc chỉ những trạng thái "visible"
                var visibleStatuses = paymentStatuses
                    .Where(p => p.IsVisible)
                    .ToDictionary(p => p.PaymentStatusName, p => p.PaymentStatusId);

                // Chuyển đổi sang JSON
                var jsonData = JsonSerializer.Serialize(visibleStatuses);

                // Cập nhật cache
                await _cacheService.SetCachedValueAsync(MappingCacheKey, jsonData, CacheExpiry);

                LogException.LogInformation("PaymentStatus cache updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
