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
        private const string CacheKey = "ServiceService";
        private const string WDCacheKey = "walkDogService";
        private const string ComboServiceCacheKey = "ComboService";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public ServiceService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<ServiceVariantDTO?> GetServiceVariantByKey(Guid serviceId, Guid breedId, string petWeightRange)
        {
            var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"Fetching service variant ({serviceId}, {breedId}, {petWeightRange}) from cache.");

                var cachedVariants = JsonSerializer.Deserialize<List<ServiceVariantDTO>>(cachedData);
                if (cachedVariants != null)
                {
                    var variant = cachedVariants.FirstOrDefault(v =>
                        v.ServiceId == serviceId &&
                        v.BreedId == breedId &&
                        v.PetWeightRange.Trim().ToLower().Equals(petWeightRange.Trim().ToLower()));

                    if (variant != null)
                    {
                        return variant;
                    }
                }
            }

            var WDcachedData = await _cacheService.GetCachedValueAsync(WDCacheKey);
            if (!string.IsNullOrEmpty(WDcachedData))
            {
                LogException.LogInformation($"Fetching walk dog service {serviceId} from cache.");

                var wdCache = JsonSerializer.Deserialize<List<ServiceVariantDTO>>(WDcachedData);

                if (wdCache != null)
                {
                    var walkDog = wdCache.FirstOrDefault(c =>
                        c.ServiceId == serviceId && c.BreedId == breedId
                    );

                    return walkDog;
                }
            }

            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];
            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("Service Service URL is not configured.");
                return null;
            }

            var apiUrl = $"{serviceServiceUrl}service-variants";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            LogException.LogInformation($"API URL: {apiUrl}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service variants. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Fetching all service variants from API...");
            LogException.LogInformation(json);

            var jsonNode = JsonNode.Parse(json);
            var serviceVariantsJson = jsonNode?["data"]?.ToJsonString();

            if (string.IsNullOrEmpty(serviceVariantsJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return null;
            }

            var serviceVariants = JsonSerializer.Deserialize<List<ServiceVariantDTO>>(serviceVariantsJson, options);
            if (serviceVariants == null || !serviceVariants.Any())
            {
                LogException.LogError("Failed to deserialize service variant data or empty list.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(serviceVariants), CacheExpiry);
            LogException.LogInformation("Service variant list cached in Redis.");

            return serviceVariants.FirstOrDefault(v =>
                v.ServiceId == serviceId &&
                v.BreedId == breedId &&
                v.PetWeightRange == petWeightRange
            );
        }


        public async Task<ServiceComboVariant?> GetServiceComboVariantByKey(Guid serviceId, Guid BreedId, string PetWeightRange)
        {
            // Kiểm tra cache
            var cachedData = await _cacheService.GetCachedValueAsync(ComboServiceCacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"Fetching service combo {serviceId} from cache.");

                var cachedCombos = JsonSerializer.Deserialize<List<ServiceComboVariant>>(cachedData);

                if (cachedCombos != null)
                {
                    var combo = cachedCombos.FirstOrDefault(c =>
                        c.ServiceComboId == serviceId
                        && c.BreedId == BreedId
                        && c.WeightRange.Trim().ToLower().Equals(PetWeightRange.ToLower().Trim()));

                    return combo;
                }
            }

            // Cache khong co data
            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];

            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("Service URL is not configured.");
                return null;
            }

            var apiUrl = $"{serviceServiceUrl}service-combos";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            LogException.LogInformation($"API URL: {apiUrl}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service combos. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Fetching all service combos from API...");
            LogException.LogInformation(json);

            var jsonNode = JsonNode.Parse(json);
            var serviceCombosJson = jsonNode?["data"]?.ToJsonString();

            if (string.IsNullOrEmpty(serviceCombosJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return null;
            }

            var serviceCombos = JsonSerializer.Deserialize<List<ServiceComboVariant>>(serviceCombosJson, options);
            if (serviceCombos == null || !serviceCombos.Any())
            {
                LogException.LogError("Failed to deserialize service combo data or empty list.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(ComboServiceCacheKey, JsonSerializer.Serialize(serviceCombos), CacheExpiry);
            LogException.LogInformation("Service combo list cached in Redis.");

            return serviceCombos.FirstOrDefault(c => c.ServiceComboId == serviceId);
        }
    }
}
