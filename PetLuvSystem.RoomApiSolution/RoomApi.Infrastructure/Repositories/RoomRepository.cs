using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Application.DTOs.Conversions;
using RoomApi.Application.Interfaces;
using RoomApi.Domain.Entities;
using RoomApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace RoomApi.Infrastructure.Repositories
{
    public class RoomRepository(RoomDbContext _context) : IRoom
    {
        public async Task<Response> CreateAsync(Room entity)
        {
            try
            {
                var existingRoom = await GetByAsync(x => x.RoomName == entity.RoomName && x.RoomTypeId == entity.RoomTypeId);

                if (existingRoom.Data is not null)
                {
                    return new Response(false, 409, "Room already exists");
                }

                await _context.Rooms.AddAsync(entity);
                await _context.SaveChangesAsync();

                await _context.Entry(entity).Reference(r => r.RoomType).LoadAsync();

                var (responseData, _) = RoomConversion.FromEntity(entity, null);

                return new Response(true, 201, "Room created successfully")
                {
                    Data = new { data = responseData }
                };
            }
            catch (DbUpdateException ex) // Lỗi khi thực hiện cập nhật database
            {
                // Log chi tiết lỗi
                var errorMessage = $"DbUpdateException: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $"\nInner-Inner Exception: {ex.InnerException.InnerException.Message}";
                    }
                }

                LogException.LogExceptions(ex); // Gọi hàm log
                throw new Exception(errorMessage); // Throw lỗi để hiển thị
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var room = await FindById(id, false, true, true);

                if (room is null)
                {
                    return new Response(false, 404, "Room not found");
                }

                var (responseData, _) = RoomConversion.FromEntity(room, null);

                if (room.IsVisible == true)
                {
                    room.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Room deleted successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Room deleted permenantly successfully")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var rooms = await _context.Rooms
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(r => r.RoomType)
                    .Include(r => r.RoomImages)
                    .ToListAsync();

                if (rooms is null || rooms.Count == 0)
                {
                    return new Response(false, 404, "No rooms found");
                }

                var (_, responseData) = RoomConversion.FromEntity(null, rooms);

                return new Response(true, 200, "Rooms found")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<Room, bool>> predicate)
        {
            try
            {
                var existingRoom = await _context.Rooms.FirstOrDefaultAsync(predicate);

                if (existingRoom is null)
                {
                    return new Response(false, 404, "Room not found");
                }

                var (responseData, _) = RoomConversion.FromEntity(existingRoom, null);

                return new Response(true, 200, "Room found")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var room = await FindById(id, true, true, true);

                if (room is null)
                {
                    return new Response(false, 404, "Room not found");
                }

                var (responseData, _) = RoomConversion.FromEntity(room, null);

                return new Response(true, 200, "Room found")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, Room entity)
        {
            try
            {
                var room = await FindById(id, false, true, true);

                if (room is null)
                {
                    return new Response(false, 404, "Room not found");
                }

                bool hasChanges =
                    room.RoomName != entity.RoomName ||
                    room.RoomDesc != entity.RoomDesc ||
                    room.PricePerHour != entity.PricePerHour ||
                    room.PricePerDay != entity.PricePerDay ||
                    room.IsVisible != entity.IsVisible ||
                    room.RoomTypeId != entity.RoomTypeId ||
                    room.RoomImages != entity.RoomImages;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes detected");
                }

                room.RoomName = entity.RoomName;
                room.RoomDesc = entity.RoomDesc;
                room.PricePerHour = entity.PricePerHour;
                room.PricePerDay = entity.PricePerDay;
                room.IsVisible = entity.IsVisible;
                room.RoomTypeId = entity.RoomTypeId;
                room.RoomImages = entity.RoomImages;

                await _context.SaveChangesAsync();

                var (responseData, _) = RoomConversion.FromEntity(room, null);

                return new Response(true, 200, "Room updated successfully")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Room> FindById(Guid id, bool noTracking = false, bool includeImages = false, bool includeType = false)
        {
            var query = _context.Rooms.Where(x => x.RoomId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeImages)
            {
                query = query.Include(x => x.RoomImages);
            }

            if (includeType)
            {
                query = query.Include(x => x.RoomType);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
