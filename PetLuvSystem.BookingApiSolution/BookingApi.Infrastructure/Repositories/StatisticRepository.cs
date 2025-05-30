﻿using BookingApi.Application.Interfaces;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace BookingApi.Infrastructure.Repositories
{
    public class StatisticRepository(BookingDbContext _context, IServiceService _serviceCacheService, IRoomService _roomService, IBreedMappingService _breedMappingService) : IStatistic
    {
        public async Task<Response> GetServicesBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year, string serviceType = "service")
        {
            try
            {
                var type = serviceType.Trim().ToLower();

                if (type.Contains("service"))
                {
                    var query = _context.ServiceBookingDetails
                        .Where(sbd => sbd.Booking != null)
                        .AsQueryable();

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(sbd =>
                            sbd.Booking.BookingStartTime.Date >= startDate.Value.Date && sbd.Booking.BookingStartTime.Date <= endDate.Value.Date ||
                            sbd.Booking.BookingEndTime.Date >= startDate.Value.Date && sbd.Booking.BookingEndTime.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue)
                    {
                        query = query.Where(sbd =>
                            sbd.Booking.BookingStartTime.Date == startDate.Value.Date ||
                            sbd.Booking.BookingEndTime.Date == startDate.Value.Date);
                    }

                    if (month.HasValue)
                    {
                        query = query.Where(sbd =>
                            sbd.Booking.BookingStartTime.Month == month.Value ||
                            sbd.Booking.BookingEndTime.Month == month.Value);
                    }

                    if (year.HasValue)
                    {
                        query = query.Where(sbd =>
                            sbd.Booking.BookingStartTime.Year == year.Value ||
                            sbd.Booking.BookingEndTime.Year == year.Value);
                    }

                    var stats = await query
                        .GroupBy(sbd => sbd.ServiceId)
                        .Select(g => new { Id = g.Key, Count = g.Count() })
                        .ToListAsync();

                    if (!stats.Any())
                        return new Response(false, 404, "Không tìm thấy booking nào");

                    var ids = stats.Select(x => x.Id).ToList();
                    var mappings = await _serviceCacheService.GetServiceMappings(ids);

                    var result = stats.Select(s => new
                    {
                        ServiceId = s.Id,
                        ServiceName = mappings.TryGetValue(s.Id, out var name) ? name : "N/A",
                        s.Count
                    }).ToList();

                    return new Response(true, 200, "OK") { Data = result };
                }
                else if (type.Contains("combo"))
                {
                    var query = _context.ServiceComboBookingDetails
                        .Where(scbd => scbd.Booking != null)
                        .AsQueryable();

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(scbd =>
                            scbd.Booking.BookingStartTime.Date >= startDate.Value.Date && scbd.Booking.BookingStartTime.Date <= endDate.Value.Date ||
                            scbd.Booking.BookingEndTime.Date >= startDate.Value.Date && scbd.Booking.BookingEndTime.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue)
                    {
                        query = query.Where(scbd =>
                            scbd.Booking.BookingStartTime.Date == startDate.Value.Date ||
                            scbd.Booking.BookingEndTime.Date == startDate.Value.Date);
                    }

                    if (month.HasValue)
                    {
                        query = query.Where(scbd =>
                            scbd.Booking.BookingStartTime.Month == month.Value ||
                            scbd.Booking.BookingEndTime.Month == month.Value);
                    }

                    if (year.HasValue)
                    {
                        query = query.Where(scbd =>
                            scbd.Booking.BookingStartTime.Year == year.Value ||
                            scbd.Booking.BookingEndTime.Year == year.Value);
                    }

                    var stats = await query
                        .GroupBy(scbd => scbd.ServiceComboId)
                        .Select(g => new { Id = g.Key, Count = g.Count() })
                        .ToListAsync();

                    if (!stats.Any())
                        return new Response(false, 404, "Không tìm thấy booking nào");

                    var ids = stats.Select(x => x.Id).ToList();
                    var mappings = await _serviceCacheService.GetServiceMappings(ids);

                    var result = stats.Select(s => new
                    {
                        ServiceComboId = s.Id,
                        ServiceName = mappings.TryGetValue(s.Id, out var name) ? name : "N/A",
                        s.Count
                    }).ToList();

                    return new Response(true, 200, "OK") { Data = result };
                }
                else
                {
                    var query = _context.RoomBookingItems
                        .Where(rbi => rbi.Booking != null)
                        .AsQueryable();

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query = query.Where(rbi =>
                            rbi.Booking.BookingStartTime.Date >= startDate.Value.Date && rbi.Booking.BookingStartTime.Date <= endDate.Value.Date ||
                            rbi.Booking.BookingEndTime.Date >= startDate.Value.Date && rbi.Booking.BookingEndTime.Date <= endDate.Value.Date);
                    }
                    else if (startDate.HasValue)
                    {
                        query = query.Where(rbi =>
                            rbi.Booking.BookingStartTime.Date == startDate.Value.Date ||
                            rbi.Booking.BookingEndTime.Date == startDate.Value.Date);
                    }

                    if (month.HasValue)
                    {
                        query = query.Where(rbi =>
                            rbi.Booking.BookingStartTime.Month == month.Value ||
                            rbi.Booking.BookingEndTime.Month == month.Value);
                    }

                    if (year.HasValue)
                    {
                        query = query.Where(rbi =>
                            rbi.Booking.BookingStartTime.Year == year.Value ||
                            rbi.Booking.BookingEndTime.Year == year.Value);
                    }

                    var stats = await query
                        .GroupBy(rbi => rbi.RoomId)
                        .Select(g => new { Id = g.Key, Count = g.Count() })
                        .ToListAsync();

                    if (!stats.Any())
                        return new Response(false, 404, "Không tìm thấy booking nào");

                    var ids = stats.Select(x => x.Id).ToList();
                    var mappings = await _roomService.GetRoomMappings(ids);

                    var result = stats.Select(s => new
                    {
                        RoomId = s.Id,
                        RoomName = mappings.TryGetValue(s.Id, out var name) ? name : "N/A",
                        s.Count
                    }).ToList();

                    return new Response(true, 200, "OK") { Data = result };
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetBreedsBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year)
        {
            try
            {
                var query = _context.ServiceBookingDetails
                    .Where(sbd => sbd.Booking != null)
                    .AsQueryable();

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(sbd =>
                        sbd.Booking.BookingStartTime.Date >= startDate.Value.Date && sbd.Booking.BookingStartTime.Date <= endDate.Value.Date ||
                        sbd.Booking.BookingEndTime.Date >= startDate.Value.Date && sbd.Booking.BookingEndTime.Date <= endDate.Value.Date);
                }
                else if (startDate.HasValue)
                {
                    query = query.Where(sbd =>
                        sbd.Booking.BookingStartTime.Date == startDate.Value.Date ||
                        sbd.Booking.BookingEndTime.Date == startDate.Value.Date);
                }

                if (month.HasValue)
                {
                    query = query.Where(sbd =>
                        sbd.Booking.BookingStartTime.Month == month.Value ||
                        sbd.Booking.BookingEndTime.Month == month.Value);
                }

                if (year.HasValue)
                {
                    query = query.Where(sbd =>
                        sbd.Booking.BookingStartTime.Year == year.Value ||
                        sbd.Booking.BookingEndTime.Year == year.Value);
                }

                var bookingBreedStats = await query
                    .GroupBy(sbd => sbd.BreedId)
                    .Select(g => new
                    {
                        BreedId = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                if (!bookingBreedStats.Any())
                {
                    return new Response(false, 404, "Không tìm thấy booking nào");
                }

                var breedMapping = await _breedMappingService.GetBreedMappingAsync();

                var result = bookingBreedStats.Select(b => new
                {
                    b.BreedId,
                    BreedName = breedMapping.TryGetValue(b.BreedId, out var name) ? name : string.Empty,
                    b.Count
                });

                return new Response(true, 200, "OK") { Data = result };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetRevenue(DateTime? startDate, DateTime? endDate, int? month, int? year)
        {
            try
            {
                var query = _context.Bookings
                    .Where(b => b.BookingStatus.BookingStatusName == "Đã hoàn thành") // Filter completed bookings
                    .AsQueryable();

                List<object> revenueStats = new();

                if (startDate.HasValue && endDate.HasValue)
                {
                    // **Stat by range (Revenue by Day)**
                    query = query.Where(b => b.BookingEndTime.Date >= startDate.Value.Date && b.BookingEndTime.Date <= endDate.Value.Date);

                    var revenueData = await query
                        .GroupBy(b => b.BookingEndTime.Date)
                        .Select(g => new
                        {
                            Day = g.Key, // Group by Date
                            Revenue = g.Sum(b => b.TotalAmount)
                        })
                        .ToListAsync();

                    // Ensure all days in range appear (Fill missing days with 0 revenue)
                    revenueStats = Enumerable.Range(0, (endDate.Value - startDate.Value).Days + 1)
                        .Select(offset => new
                        {
                            Day = startDate.Value.AddDays(offset).Date,
                            Revenue = revenueData.FirstOrDefault(r => r.Day == startDate.Value.AddDays(offset).Date)?.Revenue ?? 0
                        })
                        .Cast<object>()
                        .ToList();
                }
                else if (month.HasValue && year.HasValue)
                {
                    // **Stat by Month (Revenue by Day)**
                    query = query.Where(b => b.BookingEndTime.Month == month.Value && b.BookingEndTime.Year == year.Value);

                    var revenueData = await query
                        .GroupBy(b => b.BookingEndTime.Date)
                        .Select(g => new
                        {
                            Day = g.Key,
                            Revenue = g.Sum(b => b.TotalAmount)
                        })
                        .ToListAsync();

                    // Ensure all days in month appear
                    var daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
                    revenueStats = Enumerable.Range(1, daysInMonth)
                        .Select(day => new
                        {
                            Day = new DateTime(year.Value, month.Value, day),
                            Revenue = revenueData.FirstOrDefault(r => r.Day == new DateTime(year.Value, month.Value, day))?.Revenue ?? 0
                        })
                        .Cast<object>()
                        .ToList();
                }
                else if (year.HasValue)
                {
                    // **Stat by Year (Revenue by Month)**
                    query = query.Where(b => b.BookingEndTime.Year == year.Value);

                    var revenueData = await query
                        .GroupBy(b => b.BookingEndTime.Month)
                        .Select(g => new
                        {
                            Month = g.Key,
                            Revenue = g.Sum(b => b.TotalAmount)
                        })
                        .ToListAsync();

                    // Ensure all 12 months appear
                    revenueStats = Enumerable.Range(1, 12)
                        .Select(m => new
                        {
                            Month = m,
                            Revenue = revenueData.FirstOrDefault(r => r.Month == m)?.Revenue ?? 0
                        })
                        .Cast<object>()
                        .ToList();
                }
                else
                {
                    return new Response(false, 400, "Invalid parameters. Provide a valid date range, month, or year.");
                }

                return new Response(true, 200, "OK") { Data = revenueStats };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

    }
}
