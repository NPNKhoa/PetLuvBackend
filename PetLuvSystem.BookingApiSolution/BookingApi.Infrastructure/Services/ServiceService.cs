using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookingApi.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "getserviceById";
        private const string VariantCacheKey = "getVariantByKey";
        private const string ComboCacheKey = "getserviceById";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public ServiceService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }
        public async Task<ServiceComboVariant?> GetServiceComboVariantById(Guid serviceId)
        {
            // Check cache
            var cachedData = await _cacheService.GetCachedValueAsync(ComboCacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"service {serviceId} returned from Redis cache.");

                return JsonSerializer.Deserialize<ServiceComboVariant>(cachedData);
            }

            // Cache Missed
            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];
            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("service Service URL is not configured.");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{serviceServiceUrl}service-combo/{serviceId}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service data. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            LogException.LogInformation(json);

            var serviceDto = JsonSerializer.Deserialize<ServiceComboVariant>(json, options);
            if (serviceDto == null)
            {
                LogException.LogError("Failed to deserialize service data.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(ComboCacheKey, JsonSerializer.Serialize(serviceDto), CacheExpiry);
            LogException.LogInformation($"service {serviceId} fetched from service service API and cached in Redis.");

            return serviceDto;
        }

        public async Task<ServiceVariantDTO?> GetServiceVariantById(Guid serviceId)
        {
            // Check cache
            var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"service {serviceId} returned from Redis cache.");

                return JsonSerializer.Deserialize<ServiceVariantDTO>(cachedData);
            }

            // Cache Missed
            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];
            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("service Service URL is not configured.");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{serviceServiceUrl}services/{serviceId}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service data. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Checking Service...");
            LogException.LogInformation(json);

            var serviceDto = JsonSerializer.Deserialize<ServiceVariantDTO>(json, options);
            if (serviceDto == null)
            {
                LogException.LogError("Failed to deserialize service data.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(serviceDto), CacheExpiry);
            LogException.LogInformation($"service {serviceId} fetched from service service API and cached in Redis.");

            return serviceDto;
        }

        public async Task<ServiceVariantDTO?> GetServiceVariantByKey(Guid serviceId, Guid breedId, string petWeightRange)
        {
            var cachedData = await _cacheService.GetCachedValueAsync(VariantCacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"Service Variant ({serviceId}, {breedId}, {petWeightRange}) returned from Redis cache.");
                return JsonSerializer.Deserialize<ServiceVariantDTO>(cachedData);
            }

            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];
            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("Service Service URL is not configured.");
                return null;
            }

            var apiUrl = $"{serviceServiceUrl}service-variants/{serviceId}/{breedId}/{Uri.EscapeDataString(petWeightRange)}";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);
            LogException.LogError($"API URL: {apiUrl}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service variant data. Status code: {response.StatusCode} - {response.Content}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Checking Service Variant...");
            LogException.LogInformation(json);

            var jsonNode = JsonNode.Parse(json);
            var serviceVariantJson = jsonNode?["data"]?["data"]?.ToJsonString();

            if (string.IsNullOrEmpty(serviceVariantJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return null;
            }

            var serviceVariantDto = JsonSerializer.Deserialize<ServiceVariantDTO>(serviceVariantJson, options);
            if (serviceVariantDto == null)
            {
                LogException.LogError("Failed to deserialize service variant data.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(VariantCacheKey, JsonSerializer.Serialize(serviceVariantDto), CacheExpiry);
            LogException.LogInformation($"Service Variant ({serviceId}, {breedId}, {petWeightRange}) fetched from service API and cached in Redis.");

            return serviceVariantDto;
        }

    }
}
