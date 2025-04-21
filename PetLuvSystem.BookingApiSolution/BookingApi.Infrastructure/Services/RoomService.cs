using BookingApi.Application.DTOs.ExternalEntities;
using BookingApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using PetLuvSystem.SharedLibrary.Logs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BookingApi.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _cacheService;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "getRoomById";
        private const string RoomMappingKey = "RoomMappingKey";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(24);

        public RoomService(IHttpClientFactory httpClientFactory, IRedisCacheService cacheService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<RoomDTO?> GetRoomById(Guid roomId)
        {
            // Kiểm tra cache
            var cachedData = await _cacheService.GetCachedValueAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                LogException.LogInformation($"Fetching room {roomId} from cache.");

                // Deserialize danh sách các phòng
                var cachedRooms = JsonSerializer.Deserialize<List<RoomDTO>>(cachedData);
                if (cachedRooms != null)
                {
                    var room = cachedRooms.FirstOrDefault(r => r.RoomId == roomId);
                    if (room != null)
                    {
                        return room;
                    }
                }
            }

            // Nếu không tìm thấy trong cache, gọi API để lấy toàn bộ danh sách phòng
            var roomServiceUrl = _configuration["ExternalServices:RoomServiceUrl"];
            if (string.IsNullOrEmpty(roomServiceUrl))
            {
                LogException.LogError("Room Service URL is not configured.");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(roomServiceUrl); // Gọi API lấy toàn bộ danh sách phòng

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch room data. Status code: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var jsonNode = JsonNode.Parse(json);
            if (jsonNode == null)
            {
                LogException.LogError("Failed to parse API response.");
                return null;
            }

            var roomDataJson = jsonNode?["data"]?["data"]?.ToJsonString();
            if (string.IsNullOrEmpty(roomDataJson))
            {
                LogException.LogError("Failed to extract pet data from API response.");
                return null;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Fetching all rooms from Room service API...");
            LogException.LogInformation(roomDataJson);

            var rooms = JsonSerializer.Deserialize<List<RoomDTO>>(roomDataJson, options);
            if (rooms == null || !rooms.Any())
            {
                LogException.LogError("Failed to deserialize room data or empty list.");
                return null;
            }

            // Lưu toàn bộ danh sách phòng vào cache
            await _cacheService.SetCachedValueAsync(CacheKey, JsonSerializer.Serialize(rooms), CacheExpiry);
            LogException.LogInformation("Room list cached in Redis.");

            // Tìm phòng theo roomId trong danh sách vừa lấy từ API
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public async Task<Dictionary<Guid, string>> GetRoomMappings(List<Guid> roomIds)
        {
            var cacheData = await _cacheService.GetCachedValueAsync(RoomMappingKey);
            Dictionary<Guid, string> roomMappings = new();

            if (cacheData is not null)
            {
                LogException.LogInformation("[Booking Service] Fetching Room Mapping From Cache");

                var cachedRooms = JsonSerializer.Deserialize<List<RoomMappingDTO>>(cacheData);
                if (cachedRooms is not null)
                {
                    roomMappings = cachedRooms.ToDictionary(r => r.RoomId, r => r.RoomName);
                }

                // Check if all rooms exist in the cache
                if (roomIds.All(id => roomMappings.ContainsKey(id)))
                {
                    return roomMappings;
                }
            }

            // Rooms missing from cache, fetch from API
            var roomServiceUrl = _configuration["ExternalServices:RoomServiceUrl"];
            if (string.IsNullOrEmpty(roomServiceUrl))
            {
                LogException.LogError("Room Service URL is not configured.");
                return roomMappings;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{roomServiceUrl}");

            if (!response.IsSuccessStatusCode)
            {
                LogException.LogError($"Failed to fetch room names. Status code: {response.StatusCode}");
                return roomMappings;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonNode = JsonNode.Parse(json);
            var roomsJson = jsonNode?["data"]?["data"]?.ToJsonString();

            if (string.IsNullOrEmpty(roomsJson))
            {
                LogException.LogError("Failed to extract 'data' field from API response.");
                return roomMappings;
            }

            var rooms = JsonSerializer.Deserialize<List<RoomMappingDTO>>(roomsJson, options);
            if (rooms == null || !rooms.Any())
            {
                LogException.LogError("Failed to deserialize room data.");
                return roomMappings;
            }

            // Update Cache with new data
            await _cacheService.SetCachedValueAsync(RoomMappingKey, JsonSerializer.Serialize(rooms), CacheExpiry);
            LogException.LogInformation("Room list cached in Redis.");

            return rooms.ToDictionary(r => r.RoomId, r => r.RoomName);
        }

    }
}
