using BookingApi.Application.DTOs.Conversions;
using BookingApi.Application.Interfaces;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace BookingApi.Infrastructure.Repositories
{
    public class BookingTypeRepository(BookingDbContext _context) : IBookingType
    {
        public async Task<Response> CreateAsync(BookingType entity)
        {
            try
            {
                var existingEntity = await _context.BookingTypes.FirstOrDefaultAsync(b => b.BookingTypeName == entity.BookingTypeName);

                if (existingEntity is not null)
                {
                    return new Response(false, 409, "Đã tồn tại Loại booking này");
                }

                entity.BookingTypeName = entity.BookingTypeName.Trim();
                entity.BookingTypeDesc = entity.BookingTypeDesc.Trim();

                await _context.BookingTypes.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (response, _) = BookingTypeConversion.FromEntity(entity, null);

                return new Response(true, 200, "OK")
                {
                    Data = response
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
                var existingEntity = await FindById(id, false, true);

                if (existingEntity is null)
                {
                    return new Response(false, 404, "Loại booking này không tồn tại");
                }

                if (existingEntity.IsVisible)
                {
                    existingEntity.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(false, 200, "Ẩn Loại booking này thành công");
                }

                if (existingEntity.Bookings.Any())
                {
                    foreach (var item in existingEntity.Bookings)
                    {
                        item.BookingTypeId = Guid.Empty;
                        item.BookingType = null!;
                    }
                }

                _context.BookingTypes.Remove(existingEntity);
                await _context.SaveChangesAsync();

                return new Response(false, 200, "Xóa thành công");
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
                var entities = await _context.BookingTypes
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!entities.Any())
                {
                    return new Response(false, 404, "Không thấy Loại booking nào");
                }

                var (_, response) = BookingTypeConversion.FromEntity(null, entities);

                return new Response(true, 200, "Found")
                {
                    Data = new
                    {
                        data = response,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPage = Math.Ceiling((double)entities.Count / pageSize)
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<BookingType, bool>> predicate)
        {
            try
            {
                var entity = await _context.BookingTypes.FirstOrDefaultAsync(predicate);

                if (entity is null || !entity.IsVisible)
                {
                    return new Response(false, 404, "Loại booking này không tồn tại hoặc đã bị xóa");
                }

                var (response, _) = BookingTypeConversion.FromEntity(entity, null);

                return new Response(true, 200, "Found")
                {
                    Data = response
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
                var entity = await FindById(id, true, true);

                if (entity is null || !entity.IsVisible)
                {
                    return new Response(false, 404, "Loại booking này không tồn tại hoặc đã bị xóa");
                }

                var (response, _) = BookingTypeConversion.FromEntity(entity, null);

                return new Response(true, 200, "Found")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, BookingType entity)
        {
            try
            {
                var existingEntity = await FindById(id, false, true);

                if (existingEntity is null || !existingEntity.IsVisible)
                {
                    return new Response(false, 404, "Loại booking này không tồn tại hoặc đã bị xóa");
                }

                bool hasChanges = existingEntity.IsVisible != entity.IsVisible
                    || !existingEntity.BookingTypeName.ToLower().Equals(entity.BookingTypeName.Trim().ToLower())
                    || !existingEntity.BookingTypeDesc.ToLower().Equals(entity.BookingTypeDesc.Trim().ToLower());

                existingEntity.BookingTypeName = entity.BookingTypeName.Trim();
                existingEntity.BookingTypeDesc = entity.BookingTypeDesc.Trim();
                existingEntity.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (response, _) = BookingTypeConversion.FromEntity(existingEntity, null);

                return new Response(true, 200, "OK")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<BookingType> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.BookingTypes.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(b => b.Bookings);
            }

            return await query.FirstOrDefaultAsync(b => b.BookingTypeId == id) ?? null!;
        }
    }
}
