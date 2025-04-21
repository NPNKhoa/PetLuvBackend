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
    public class BookingRepository(BookingDbContext _context, ICheckPaymentStatusService _paymentStatusService) : IBooking
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
                    .Include(b => b.BookingStatus)
                    .Include(b => b.BookingType)
                    .Include(b => b.ServiceBookingDetails)
                    .Include(b => b.ServiceComboBookingDetails)
                    .Include(b => b.RoomBookingItem)
                    .OrderByDescending(b => b.BookingStartTime)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!entities.Any())
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 404, "Không tìm thấy booking nào");
                }

                LogException.LogInformation("Converting...");

                var (_, response) = await BookingConversion.FromEntity(null, entities, _paymentStatusService);

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

                string paymentStatusName = await _paymentStatusService.GetPaymentStatusNameById(entity.PaymentStatusId);

                var (response, _) = await BookingConversion.FromEntity(entity, null, _paymentStatusService);

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

                LogException.LogInformation($"[Booking Service] Booking Service Items Count {entity.ServiceBookingDetails?.Count}");

                var (response, _) = await BookingConversion.FromEntity(entity, null, _paymentStatusService);

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
                if (entity.BookingStartTime >= entity.BookingEndTime)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 400, "Thời gian lịch hẹn không hợp lệ");
                }

                var existingEntity = await _context.Bookings.Include(b => b.BookingStatus).Include(b => b.BookingType).FirstOrDefaultAsync(b => b.BookingId == id);

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

                existingEntity.RoomRentalTime =
                    existingEntity.RoomRentalTime != entity.RoomRentalTime ?
                    entity.RoomRentalTime : existingEntity.RoomRentalTime;

                existingEntity.TotalEstimateTime =
                    existingEntity.TotalEstimateTime != entity.TotalEstimateTime
                    || entity.TotalEstimateTime is not null
                    ? entity.TotalEstimateTime : existingEntity.TotalEstimateTime;

                existingEntity.BookingStatusId =
                    existingEntity.BookingStatusId != entity.BookingStatusId ?
                    entity.BookingStatusId : existingEntity.BookingStatusId;

                existingEntity.PaymentStatusId =
                    existingEntity.PaymentStatusId != entity.PaymentStatusId ?
                    entity.PaymentStatusId : existingEntity.PaymentStatusId;


                LogException.LogInformation("Booking Status Id neeeeeeeeeeee: " + entity.BookingStatusId.ToString());

                await _context.SaveChangesAsync();

                var (response, _) = await BookingConversion.FromEntity(existingEntity, null, _paymentStatusService);

                return new PetLuvSystem.SharedLibrary.Responses.Response(true, 200, "OK")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {

                if (ex.InnerException is not null)
                {
                    LogException.LogExceptions(ex.InnerException);
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 500, "Internal Server Error");
                }

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
                query = query
                    .Include(b => b.BookingType)
                    .Include(b => b.BookingStatus)
                    .Include(b => b.RoomBookingItem)
                    .Include(b => b.ServiceComboBookingDetails);
            }

            var booking = await query.FirstOrDefaultAsync(b => b.BookingId == id) ?? null!;
            if (booking == null) return null!;

            booking.ServiceBookingDetails = await _context.ServiceBookingDetails
                .Where(x => x.BookingId == id)
                .ToListAsync();

            LogException.LogInformation($"aaaaaaaaaaaaaaaaa {booking.ServiceBookingDetails.Count}");

            return booking;
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
                    && b.PetId == entity.PetId
                    && (b.BookingStartTime == entity.BookingStartTime
                            || b.BookingEndTime >= entity.BookingStartTime
                        )
                    && !b.BookingStatus.BookingStatusName.ToLower().Contains("hủy")
                    );

                if (existingEntity is not null)
                {
                    return new PetLuvSystem.SharedLibrary.Responses.Response(false, 409, "Đã tồn tại lịch hẹn cho thú cưng này trong khoảng thời gian này");
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

        public async Task<Response> GetBookingHistory(Guid userId)
        {
            try
            {
                var bookings = await _context.Bookings
                                        .Include(b => b.BookingStatus)
                                        .Include(b => b.BookingType)
                                        .OrderByDescending(b => b.BookingStartTime)
                                        .Where(b => b.CustomerId == userId)
                                        .ToListAsync();

                if (bookings is null || bookings.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thầy lịch hẹn nào");
                }

                var (_, response) = await BookingConversion.FromEntity(null, bookings, _paymentStatusService);

                return new Response(true, 200, "Found")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null)
                {
                    LogException.LogExceptions(ex.InnerException);
                }
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }
    }
}
