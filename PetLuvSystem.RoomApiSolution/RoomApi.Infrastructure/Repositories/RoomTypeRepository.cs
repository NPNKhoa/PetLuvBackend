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
    public class RoomTypeRepository(RoomDbContext _context) : IRoomType
    {
        public async Task<Response> CreateAsync(RoomType entity)
        {
            try
            {
                var existingRoomType = await GetByAsync(x => x.RoomTypeName == entity.RoomTypeName);

                if (existingRoomType.Data is not null)
                {
                    return new Response(false, 409, "Room Type with this name already exists");
                }

                await _context.RoomTypes.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = RoomTypeConversion.FromEntity(entity, null);

                return new Response(true, 201, "Room Type Created Successfully")
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
                var exitingRoomType = await FindById(id);

                if (exitingRoomType is null)
                {
                    return new Response(false, 404, "Room Type not found");
                }

                var (responseData, _) = RoomTypeConversion.FromEntity(exitingRoomType, null);

                if (exitingRoomType.IsVisible == true)
                {
                    exitingRoomType.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Room Type was made as hidden successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                _context.RoomTypes.Remove(exitingRoomType);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Room Type was permenantly Deleted Successfully")
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
                var roomTypes = await _context.RoomTypes
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                if (roomTypes is null || roomTypes.Count == 0)
                {
                    return new Response(false, 404, "No Room Type found");
                }

                var (_, responseData) = RoomTypeConversion.FromEntity(null, roomTypes);

                return new Response(true, 200, "Room Types Retrieved successfully")
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

        public async Task<Response> GetByAsync(Expression<Func<RoomType, bool>> predicate)
        {
            try
            {
                var roomType = await _context.RoomTypes.AsNoTracking().FirstOrDefaultAsync(predicate);

                if (roomType is null)
                {
                    return new Response(false, 404, "Room Type not found");
                }

                var (responseData, _) = RoomTypeConversion.FromEntity(roomType, null);

                return new Response(true, 200, "Room Type Retrieved successfully")
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
                var roomType = await FindById(id, true, true);

                if (roomType is null)
                {
                    return new Response(false, 404, "Room Type not found");
                }

                var (responseData, _) = RoomTypeConversion.FromEntity(roomType, null);

                return new Response(true, 200, "Room Type Retrieved successfully")
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

        public async Task<Response> UpdateAsync(Guid id, RoomType entity)
        {
            try
            {
                var existingRoomType = await FindById(id);

                if (existingRoomType is null)
                {
                    return new Response(false, 404, "Room Type not found");
                }

                bool hasChanges =
                    existingRoomType.RoomTypeName != entity.RoomTypeName
                    || existingRoomType.RoomTypeDesc != entity.RoomTypeDesc
                    || existingRoomType.IsVisible != entity.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes made to Room Type");
                }

                existingRoomType.RoomTypeName = entity.RoomTypeName;
                existingRoomType.RoomTypeDesc = entity.RoomTypeDesc;
                existingRoomType.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = RoomTypeConversion.FromEntity(existingRoomType, null);

                return new Response(true, 200, "Room Type Updated Successfully")
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

        public async Task<RoomType> FindById(Guid id, bool noTracking = false, bool includeRoom = false)
        {
            var query = _context.RoomTypes.Where(x => x.RoomTypeId == id);

            if (includeRoom)
            {
                query = query.Include(x => x.Rooms);
            }

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
