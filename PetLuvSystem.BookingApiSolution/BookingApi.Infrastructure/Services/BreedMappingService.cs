using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using BookingApi.Application.Utils;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;

namespace BookingApi.Infrastructure.Services
{
    public class BreedMappingService : IBreedMappingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "breedMapping";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public BreedMappingService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<Dictionary<Guid, string>> GetBreedMappingAsync()
        {
            // Check Redis Cache
            var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation("Breed mapping returned from Redis cache.");

                var tempDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cachedData);
                var mapping = new Dictionary<Guid, string>();

                foreach (var kvp in tempDict)
                {
                    if (Guid.TryParse(kvp.Key, out Guid guid))
                    {
                        mapping[guid] = kvp.Value;
                    }
                }
                return mapping;
            }

            // No Cache
            var mappingUrl = _configuration["ExternalServices:PetServiceBreedMappingUrl"];
            if (string.IsNullOrEmpty(mappingUrl))
            {
                LogException.LogError("Pet Service Breed Mapping URL is not configured.");
                return new Dictionary<Guid, string>();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(mappingUrl);

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch breed mapping. Status code: {response.StatusCode}");
                return new Dictionary<Guid, string>();
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var mappingResponse = JsonSerializer.Deserialize<MappingResponse<BreedMappingDTO>>(json, options);

            if (mappingResponse == null || mappingResponse.Data == null)
            {
                LogException.LogError("Failed to deserialize breed mapping data.");
                return new Dictionary<Guid, string>();
            }

            LogException.LogInformation(mappingResponse.Message);
            var breedList = mappingResponse.Data;

            if (breedList == null)
            {
                LogException.LogError("Failed to deserialize breed mapping data.");
                return new Dictionary<Guid, string>();
            }

            var mappingResult = new Dictionary<Guid, string>();

            foreach (var breed in breedList)
            {
                mappingResult[breed.BreedId] = breed.BreedName;
            }

            var cacheDict = new Dictionary<string, string>();

            foreach (var kvp in mappingResult)
            {
                cacheDict[kvp.Key.ToString()] = kvp.Value;
            }

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(cacheDict), CacheExpiry);
            LogException.LogInformation("Breed mapping updated from Pet service API and cached in Redis.");

            return mappingResult;
        }
    }
}
