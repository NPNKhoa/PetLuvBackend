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
    public class RoomAccessoryRepository(RoomDbContext _context) : IRoomAccessory
    {
        public async Task<Response> CreateAsync(RoomAccessory entity)
        {
            try
            {
                var existingRoomAccessory = await GetByAsync(ra => ra.RoomAccessoryName == entity.RoomAccessoryName);

                if (existingRoomAccessory.Data is not null)
                {
                    return new Response(false, 409, "Room Accessory already exists");
                }

                await _context.RoomAccessories.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = RoomAccessoryConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Room Accessory created successfully")
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

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var roomAccessory = await FindById(id);

                if (roomAccessory is null)
                {
                    return new Response(false, 404, "Room Accessory not found");
                }

                var (responseData, _) = RoomAccessoryConversion.FromEntity(roomAccessory, null!);

                if (roomAccessory.IsVisible)
                {
                    roomAccessory.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Room Accessory was made as hidden successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                _context.RoomAccessories.Remove(roomAccessory);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Room Accessory was permenantly deleted successfully")
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

        public async Task<RoomAccessory> FindById(Guid id, bool noTracking = false, bool includeRoomType = false)
        {
            var query = _context.RoomAccessories.Where(ra => ra.RoomAccessoryId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeRoomType)
            {
                query = query.Include(r => r.RoomType);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var roomAccessories = await _context.RoomAccessories
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.RoomType)
                    .ToListAsync();

                if (roomAccessories is null || roomAccessories.Count == 0)
                {
                    return new Response(false, 404, "Room Accessory not found");
                }

                var (_, responseData) = RoomAccessoryConversion.FromEntity(null!, roomAccessories);

                return new Response(true, 200, "Room Accessory found")
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

        public async Task<Response> GetByAsync(Expression<Func<RoomAccessory, bool>> predicate)
        {
            try
            {
                var roomAccessory = await _context.RoomAccessories.Where(predicate).FirstOrDefaultAsync();

                if (roomAccessory is null)
                {
                    return new Response(false, 404, "Room Accessory not found");
                }

                var (responseData, _) = RoomAccessoryConversion.FromEntity(roomAccessory, null!);

                return new Response(true, 200, "Room Accessory found")
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
                var roomAccessory = await FindById(id, true, true);

                if (roomAccessory is null)
                {
                    return new Response(false, 404, "Room Accessory not found");
                }

                var (responseData, _) = RoomAccessoryConversion.FromEntity(roomAccessory, null!);

                return new Response(true, 200, "Room Accessory found")
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

        public async Task<Response> UpdateAsync(Guid id, RoomAccessory entity)
        {
            try
            {
                var roomAccessory = await FindById(id, false, true);

                if (roomAccessory is null)
                {
                    return new Response(false, 404, "Room Accessory not found");
                }

                bool hasChanges =
                    roomAccessory.RoomAccessoryName != entity.RoomAccessoryName ||
                    roomAccessory.RoomAccessoryDesc != entity.RoomAccessoryDesc ||
                    roomAccessory.RoomAccessoryImagePath != entity.RoomAccessoryImagePath ||
                    roomAccessory.IsVisible != entity.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes found");
                }

                roomAccessory.RoomAccessoryName = entity.RoomAccessoryName;
                roomAccessory.RoomAccessoryDesc = entity.RoomAccessoryDesc;
                roomAccessory.RoomAccessoryImagePath = entity.RoomAccessoryImagePath;
                roomAccessory.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = RoomAccessoryConversion.FromEntity(roomAccessory, null!);

                return new Response(true, 200, "Room Accessory updated successfully")
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
    }
}
