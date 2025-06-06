﻿using BookingApi.Application.DTOs.ExternalEntities;

namespace BookingApi.Application.Interfaces
{
    public interface IRoomService
    {
        public Task<RoomDTO?> GetRoomById(Guid RoomId);
        public Task<Dictionary<Guid, string>> GetRoomMappings(List<Guid> roomIds);
    }
}
