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
        private const string ServiceMappingKey = "ServiceMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public ServiceService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<Dictionary<Guid, string>> GetServiceMappings(List<Guid> serviceIds)
        {
            var cacheData = await _cacheService.GetCachedValueAsync(ServiceMappingKey);
            Dictionary<Guid, string> serviceMappings = new();

            if (cacheData is not null)
            {
                LogException.LogInformation("[Booking Service] Fetching Service Mapping From Cache");

                var cachedServices = JsonSerializer.Deserialize<List<ServiceMappingDTO>>(cacheData);
                if (cachedServices is not null)
                {
                    serviceMappings = cachedServices.ToDictionary(s => s.ServiceId, s => s.ServiceName);
                }

                // Check if all services exist in the cache
                if (serviceIds.All(id => serviceMappings.ContainsKey(id)))
                {
                    return serviceMappings;
                }
            }

            // Services missing from cache, fetch from API
            var serviceServiceUrl = _configuration["ExternalServices:ServiceServiceUrl"];
            if (string.IsNullOrEmpty(serviceServiceUrl))
            {
                LogException.LogError("Service Service URL is not configured.");
                return serviceMappings;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{serviceServiceUrl}services");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch service names. Status code: {response.StatusCode}");
                return serviceMappings;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonNode = JsonNode.Parse(json);
            var servicesJson = jsonNode?["data"]?["data"]?.ToJsonString();

            if (string.IsNullOrEmpty(servicesJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return serviceMappings;
            }

            var services = JsonSerializer.Deserialize<List<ServiceMappingDTO>>(servicesJson, options);
            if (services == null || !services.Any())
            {
                LogException.LogError("Failed to deserialize service data.");
                return serviceMappings;
            }

            // Update Cache with new data
            await _cacheService.SetCachedValueAsync(ServiceMappingKey, JsonSerializer.Serialize(services), CacheExpiry);
            LogException.LogInformation("Service list cached in Redis.");

            return services.ToDictionary(s => s.ServiceId, s => s.ServiceName);
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
