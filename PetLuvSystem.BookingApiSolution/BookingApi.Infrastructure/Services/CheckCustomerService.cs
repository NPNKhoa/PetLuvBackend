using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookingApi.Infrastructure.Services
{
    public class CheckCustomerService : ICheckCustomerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "checkCustomer";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public CheckCustomerService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<bool> CheckCustomerAsync(Guid customerId)
        {
            var cacheData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cacheData))
            {
                LogException.LogInformation("Data fetched from cache");

                var cachedUsers = JsonSerializer.Deserialize<HashSet<Guid>>(cacheData);
                if (cachedUsers != null && cachedUsers.Contains(customerId))
                {
                    return true;
                }

                return false;
            }

            LogException.LogInformation("No cache found. Attempting to get data from API...");
            var apiUrl = _configuration["ExternalServices:CheckCustomerUrl"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                LogException.LogError("Check Customer URL is not configured");
                return false;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"API call failed with status code: {response.StatusCode}");
                return false;
            }

            var json = await response.Content.ReadAsStringAsync();

            var jsonNode = JsonNode.Parse(json);
            if (jsonNode == null)
            {
                LogException.LogError("Failed to parse API response.");
                return false;
            }

            var userDataJson = jsonNode?["data"]?["data"]?.ToJsonString();
            if (string.IsNullOrEmpty(userDataJson))
            {
                LogException.LogError("Failed to extract user data from API response.");
                return false;
            }

            LogException.LogInformation("Checking Customer...");
            LogException.LogInformation(json);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<UserDTO>>(userDataJson, options);

            if (users == null)
            {
                LogException.LogError("Failed to deserialize response data.");
                return false;
            }

            var customerIds = users.Where(u => u.IsActive).Select(u => u.UserId).ToHashSet();

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(customerIds), CacheExpiry);

            LogException.LogInformation("Customer list is updated from API and cached in Redis.");

            return customerIds.Contains(customerId);
        }

    }
}
