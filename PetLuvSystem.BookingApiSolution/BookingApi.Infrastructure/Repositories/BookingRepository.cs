using BookingApi.Application.DTOs.Conversions;
using BookingApi.Application.Interfaces;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using System.Linq.Expressions;

namespace BookingApi.Infrastructure.Repositories
{
    public class BookingRepository(BookingDbContext _context) : IBooking
    {
        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> CreateAsync(Booking entity)
        {
            try
            {
                await _context.Bookings.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "OK");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex.InnerException is not null)
                {
                    LogException.LogError(ex.InnerException.Message);
                }
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var entities = await _context.Bookings
                    .OrderByDescending(b => b.BookingStartTime)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!entities.Any())
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy booking nào");
                }

                var (_, response) = BookingConversion.FromEntity(null, entities);

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "OK")
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
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> GetByAsync(Expression<Func<Booking, bool>> predicate)
        {
            try
            {
                var entity = await _context.Bookings.FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy booking này");
                }

                var (response, _) = BookingConversion.FromEntity(entity, null);

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "Found")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await FindById(id, true, true);

                if (entity is null)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy booking này");
                }

                var (response, _) = BookingConversion.FromEntity(entity, null);

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "Found")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> UpdateAsync(Guid id, Booking entity)
        {
            try
            {
                if (
                    entity.BookingStartTime > DateTime.UtcNow
                    || entity.BookingEndTime > DateTime.UtcNow
                    || entity.BookingStartTime < entity.BookingEndTime
                )
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Thời gian lịch hẹn không hợp lệ");
                }

                var existingEntity = await _context.Bookings.FirstOrDefaultAsync(b =>
                    b.CustomerId == entity.CustomerId
                    && (
                        b.RoomBookingItem != null && entity.RoomBookingItem != null
                        && (b.RoomBookingItem.RoomId == entity.RoomBookingItem.RoomId)
                    )
                    && (
                        b.ServiceBookingDetails != null
                        && b.ServiceComboBookingDetails != null
                            && b.ServiceBookingDetails.Count > 0
                            && b.ServiceComboBookingDetails.Count > 0
                    )
                    && b.PetId == entity.PetId
                    && b.BookingStartTime == entity.BookingStartTime
                    && b.BookingEndTime == entity.BookingEndTime);

                if (existingEntity is null)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy booking cần tìm");
                }

                existingEntity.BookingStartTime =
                    existingEntity.BookingStartTime != entity.BookingStartTime
                    ? entity.BookingStartTime : existingEntity.BookingStartTime;

                existingEntity.BookingEndTime =
                    existingEntity.BookingEndTime != entity.BookingEndTime ?
                    entity.BookingEndTime : existingEntity.BookingEndTime;

                existingEntity.BookingNote =
                    existingEntity.BookingNote is not null
                    && entity.BookingNote is not null
                    && !existingEntity.BookingNote.ToLower().Equals(entity.BookingNote.Trim().ToLower()) ?
                    entity.BookingNote : existingEntity.BookingNote;

                existingEntity.TotalEstimateTime =
                    existingEntity.TotalEstimateTime != entity.TotalEstimateTime ?
                    entity.TotalEstimateTime : existingEntity.TotalEstimateTime;

                existingEntity.BookingTypeId =
                    existingEntity.BookingTypeId != entity.BookingTypeId ?
                    entity.BookingTypeId : existingEntity.BookingTypeId;

                existingEntity.BookingStatusId =
                    existingEntity.BookingStatusId != entity.BookingStatusId ?
                    entity.BookingStatusId : existingEntity.BookingStatusId;

                existingEntity.PaymentStatusId =
                    existingEntity.PaymentStatusId != entity.PaymentStatusId ?
                    entity.PaymentStatusId : existingEntity.PaymentStatusId;

                existingEntity.CustomerId =
                    existingEntity.CustomerId != entity.CustomerId ?
                    entity.CustomerId : existingEntity.CustomerId;

                existingEntity.PetId =
                    existingEntity.PetId != entity.PetId ?
                    entity.PetId : existingEntity.PetId;

                await _context.SaveChangesAsync();

                var (response, _) = BookingConversion.FromEntity(existingEntity, null);

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "OK")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Booking> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.Bookings.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(b => b.BookingType)
                    .Include(b => b.BookingStatus)
                    .Include(b => b.RoomBookingItem)
                    .Include(b => b.ServiceBookingDetails)
                    .Include(b => b.ServiceComboBookingDetails);
            }

            return await query.FirstOrDefaultAsync(b => b.BookingId == id) ?? null!;
        }

        public async Task<PetLuvSystem.SharedLibrary.Responses.Response> ValidateBookingCreation(Booking entity)
        {
            try
            {
                if (
                    entity.BookingStartTime <= DateTime.UtcNow
                    || entity.BookingEndTime <= DateTime.UtcNow
                    || entity.BookingStartTime >= entity.BookingEndTime
                )
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Thời gian lịch hẹn không hợp lệ");
                }

                var existingEntity = await _context.Bookings.FirstOrDefaultAsync(b =>
                    b.CustomerId == entity.CustomerId
                    && (
                        b.RoomBookingItem != null && entity.RoomBookingItem != null
                        && (b.RoomBookingItem.RoomId == entity.RoomBookingItem.RoomId)
                    )
                    && (
                        b.ServiceBookingDetails != null
                        && b.ServiceComboBookingDetails != null
                            && b.ServiceBookingDetails.Count > 0
                            && b.ServiceComboBookingDetails.Count > 0
                    )
                    && b.PetId == entity.PetId
                    && b.BookingStartTime == entity.BookingStartTime
                    && b.BookingEndTime == entity.BookingEndTime);

                if (existingEntity is not null)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 409, "Đã tồn tại booking tương tự");
                }

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "Validation Success");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex.InnerException is not null)
                {
                    LogException.LogError(ex.InnerException.Message);
                }
                return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
            }
        }

        // Unused
        public Task<PetLuvSystem.SharedLibrary.Responses.Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
