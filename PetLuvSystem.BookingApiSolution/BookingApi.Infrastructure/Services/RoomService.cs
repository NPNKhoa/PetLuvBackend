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
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            LogException.LogInformation("Fetching all rooms from Room service API...");
            LogException.LogInformation(json);

            var rooms = JsonSerializer.Deserialize<List<RoomDTO>>(json, options);
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

    }
}
