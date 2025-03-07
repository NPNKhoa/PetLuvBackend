using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookingApi.Infrastructure.Services
{
    public class CheckPetService : ICheckPetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "checkPet";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public CheckPetService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<bool> CheckPetAsync(Guid petId)
        {
            var cacheData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cacheData))
            {
                LogException.LogInformation("Data fetched from cache");

                var cachedPets = JsonSerializer.Deserialize<HashSet<Guid>>(cacheData);
                if (cachedPets != null && cachedPets.Contains(petId))
                {
                    return true;
                }

                return false;
            }

            LogException.LogInformation("No cache found. Attempting to get data from API...");
            var apiUrl = _configuration["ExternalServices:CheckPetUrl"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                LogException.LogError("Check Pet URL is not configured");
                return false;
            }

            try
            {
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

                var petDataJson = jsonNode?["data"]?["data"]?.ToJsonString();
                if (string.IsNullOrEmpty(petDataJson))
                {
                    LogException.LogError("Failed to extract pet data from API response.");
                    return false;
                }

                LogException.LogInformation("Checking Pet...");
                LogException.LogInformation(json);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var pets = JsonSerializer.Deserialize<List<PetDTO>>(petDataJson, options);

                if (pets == null)
                {
                    LogException.LogError("Failed to deserialize response data.");
                    return false;
                }

                var petIds = pets.Where(p => p.IsVisible).Select(p => p.PetId).ToHashSet();

                await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(petIds), CacheExpiry);

                LogException.LogInformation("Pet list is updated from API and cached in Redis.");

                return petIds.Contains(petId);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return false;
            }
        }

    }
}
