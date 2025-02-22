using BookingApi.Application.Interfaces;
using BookingApi.Domain.Entities;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace BookingApi.Infrastructure.Repositories
{
    public class BookingRepository(BookingDbContext _context) : IBooking
    {
        public async Task<Response> CreateAsync(Booking entity)
        {
            try
            {
                return new Response(true, 200, "OK");
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
                var entities = await _context.Bookings
                    .OrderByDescending(b => b.BookingStartTime)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!entities.Any())
                {
                    return new Response(false, 404, "Không tìm thấy booking nào");
                }

                return new Response(true, 200, "OK");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<Booking, bool>> predicate)
        {
            try
            {
                var entity = await _context.Bookings.FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new Response(false, 404, "Không tìm thấy booking này");
                }

                return new Response(true, 200, "Found");
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

                if (entity is null)
                {
                    return new Response(false, 404, "Không tìm thấy booking này");
                }

                return new Response(true, 200, "Found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public Task<Response> UpdateAsync(Guid id, Booking entity)
        {
            throw new NotImplementedException();
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

        // Unused
        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
