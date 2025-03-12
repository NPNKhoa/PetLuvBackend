using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Utils;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace BookingApi.Infrastructure.Services
{
    public class CheckPaymentStatusService : ICheckPaymentStatusService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "checkPaymentStatus";
        private const string MappingCacheKey = "getPaymentStatusMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public CheckPaymentStatusService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<bool> CheckPaymentStatusAsync(Guid PaymentStatusId)
        {
            var cacheData = await _cacheService.GetCachedValueAsync(CacheKey);

            // Check Cache
            if (!string.IsNullOrEmpty(cacheData))
            {
                LogException.LogInformation("Data fetched from cache");

                var tempDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cacheData);
                var checkCache = new Dictionary<Guid, bool>();

                foreach (var tmp in tempDict)
                {
                    if (Guid.TryParse(tmp.Key, out Guid guid) && Boolean.TryParse(tmp.Value, out bool value))
                    {
                        checkCache[guid] = value;
                    }
                }

                if (checkCache.TryGetValue(PaymentStatusId, out bool isValid)) return isValid;

                return false;
            }

            // No Data Found In Cache
            var apiUrl = _configuration["ExternalServices:CheckPaymentStatusUrl"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                LogException.LogError("Check PaymentStatus URL is not configured");
                return false;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{apiUrl}");

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var mappingResponse = JsonSerializer.Deserialize<MappingResponse<Guid>>(json, options);

            if (mappingResponse == null || mappingResponse.Data == null)
            {
                LogException.LogError("Failed to deserialize response data.");
                return false;
            }

            LogException.LogInformation(mappingResponse.Message);
            var PaymentStatuss = mappingResponse.Data;

            if (PaymentStatuss is null)
            {
                LogException.LogError("Failed to deserialize response data.");
                return false;
            }

            var mappingResult = new Dictionary<Guid, bool>();

            foreach (var id in PaymentStatuss)
            {
                mappingResult[id] = true;
            }

            var cacheDict = new Dictionary<string, string>();

            foreach (var kvp in mappingResult)
            {
                cacheDict[kvp.Key.ToString()] = kvp.Value.ToString();
            }

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(cacheDict), CacheExpiry);

            LogException.LogInformation("PaymentStatus is updated from PaymentStatus service API and cached in Redis.");
            return true;
        }

        public async Task<Guid> GetPaymentStatusIdByName(string paymentStatusName)
        {
            // Kiểm tra cache trước
            var cacheData = await _cacheService.GetCachedValueAsync(MappingCacheKey);

            if (!string.IsNullOrEmpty(cacheData))
            {
                LogException.LogInformation("Data fetched from cache");

                var cachedStatuses = JsonSerializer.Deserialize<Dictionary<string, Guid>>(cacheData);

                if (cachedStatuses != null && cachedStatuses.TryGetValue(paymentStatusName, out Guid paymentStatusId))
                {
                    return paymentStatusId;
                }

                return Guid.Empty;
            }

            // Không tìm thấy trong cache, gọi API để lấy dữ liệu
            var apiUrl = _configuration["ExternalServices:GetPaymentStatusIdByName"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                LogException.LogError("Check PaymentStatus URL is not configured");
                return Guid.Empty;
            }

            var requestUrl = $"{apiUrl}?paymentStatusName={Uri.EscapeDataString(paymentStatusName)}";

            LogException.LogInformation(requestUrl);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    LogException.LogError($"API call failed: {response.StatusCode}");
                    return Guid.Empty;
                }

                var json = await response.Content.ReadAsStringAsync();
                LogException.LogInformation("Checking PaymentStatus...");
                LogException.LogInformation(json);

                // Deserialize JSON response
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonNode = JsonNode.Parse(json);
                var paymentStatusJson = jsonNode?["data"]?.ToString();

                if (string.IsNullOrEmpty(paymentStatusJson))
                {
                    LogException.LogError("Failed to extract 'data' field from API response.");
                    return Guid.Empty;
                }

                var paymentStatuses = JsonSerializer.Deserialize<List<PaymentStatus>>(paymentStatusJson, options);

                if (paymentStatuses == null || !paymentStatuses.Any())
                {
                    LogException.LogError("Failed to deserialize response data.");
                    return Guid.Empty;
                }

                LogException.LogInformation("PaymentStatus list retrieved from API.");

                var visibleStatuses = paymentStatuses
                    .Where(p => p.IsVisible)
                    .ToDictionary(p => p.PaymentStatusName, p => p.PaymentStatusId);

                await _cacheService.SetCachedValueAsync(MappingCacheKey, JsonSerializer.Serialize(visibleStatuses), CacheExpiry);
                LogException.LogInformation("PaymentStatus is updated from API and cached in Redis.");

                return visibleStatuses.TryGetValue(paymentStatusName, out Guid statusId) ? statusId : Guid.Empty;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return Guid.Empty;
            }
        }

        public async Task<string> GetPaymentStatusNameById(Guid paymentStatusId)
        {
            // Kiểm tra cache trước
            var cacheData = await _cacheService.GetCachedValueAsync(MappingCacheKey);

            if (!string.IsNullOrEmpty(cacheData))
            {
                LogException.LogInformation("Fetching payment statuses from cache...");

                var cachedStatuses = JsonSerializer.Deserialize<Dictionary<string, Guid>>(cacheData);
                if (cachedStatuses != null)
                {
                    var idToNameMap = cachedStatuses.ToDictionary(kv => kv.Value, kv => kv.Key); // Đảo ngược key-value
                    if (idToNameMap.TryGetValue(paymentStatusId, out string paymentStatusName))
                    {
                        return paymentStatusName;
                    }
                }
            }

            LogException.LogInformation("No cache, fetching data from api...");
            // Nếu cache không có, gọi API để lấy dữ liệu
            var apiUrl = _configuration["ExternalServices:GetPaymentStatusIdByName"];
            if (string.IsNullOrEmpty(apiUrl))
            {
                LogException.LogError("Payment Status API URL is not configured.");
                return string.Empty;
            }

            LogException.LogInformation("Fetching payment status from API...");
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"API call failed: {response.StatusCode}");
                return string.Empty;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonNode = JsonNode.Parse(json);
            var paymentStatusJson = jsonNode?["data"]?.ToString();

            if (string.IsNullOrEmpty(paymentStatusJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return string.Empty;
            }

            var paymentStatuses = JsonSerializer.Deserialize<List<PaymentStatus>>(paymentStatusJson, options);
            if (paymentStatuses == null || !paymentStatuses.Any())
            {
                LogException.LogError("Failed to deserialize response data.");
                return string.Empty;
            }

            LogException.LogInformation("Payment statuses retrieved from API.");

            var visibleStatuses = paymentStatuses
                .Where(p => p.IsVisible)
                .ToDictionary(p => p.PaymentStatusName, p => p.PaymentStatusId);

            await _cacheService.SetCachedValueAsync(MappingCacheKey, JsonSerializer.Serialize(visibleStatuses), CacheExpiry);
            LogException.LogInformation("Payment statuses updated and cached in Redis.");

            return visibleStatuses.FirstOrDefault(x => x.Value == paymentStatusId).Key ?? string.Empty;
        }

    }
}
