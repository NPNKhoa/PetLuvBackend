using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;

namespace BookingApi.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "getRoomById";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public RoomService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<RoomDTO?> GetRoomById(Guid roomId)
        {
            // Check cache
            var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"Room {roomId} returned from Redis cache.");

                return JsonSerializer.Deserialize<RoomDTO>(cachedData);
            }

            // Cache Missed
            var roomServiceUrl = _configuration["ExternalServices:RoomServiceUrl"];
            if (string.IsNullOrEmpty(roomServiceUrl))
            {
                LogException.LogError("Room Service URL is not configured.");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{roomServiceUrl}{roomId}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch room data. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Checking Room...");
            LogException.LogInformation(json);

            var roomDto = JsonSerializer.Deserialize<RoomDTO>(json, options);
            if (roomDto == null)
            {
                LogException.LogError("Failed to deserialize room data.");
                return null;
            }

            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(roomDto), CacheExpiry);
            LogException.LogInformation($"Room {roomId} fetched from Room service API and cached in Redis.");

            return roomDto;
        }

    }
}
