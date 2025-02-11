using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.Interfaces;
using UserApi.Domain.Etities;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repository
{
    public class WorkScheduleDetailRepository(UserDbContext _context) : IWorkScheduleDetail
    {
        public async Task<Response> CreateAsync(WorkScheduleDetail entity)
        {
            try
            {
                var existingSchedule = await _context.WorkScheduleDetails
                    .Where(x => x.WorkScheduleId == entity.WorkScheduleId
                        && x.WorkingDate == x.WorkingDate
                        && x.StartTime == x.StartTime
                        && x.EndTime == x.EndTime)
                    .FirstOrDefaultAsync();

                if (existingSchedule is not null)
                {
                    return new Response(false, 409, "Lịch làm này đã tồn tại");
                }

                await _context.WorkScheduleDetails.AddAsync(entity);

                await _context.SaveChangesAsync();

                var (response, _) = WorkScheduleDetailConversion.FromEntity(entity, null);

                return new Response(true, 201, "Tạo thành công")
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

        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var schedules = await _context.WorkScheduleDetails
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (schedules is null || schedules.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var totalRecords = await _context.WorkScheduleDetails.CountAsync();

                var (_, response) = WorkScheduleDetailConversion.FromEntity(null, schedules);

                return new Response(true, 200, "Thành công")
                {
                    Data = new
                    {
                        data = response,
                        currentPage = pageIndex,
                        totalPages = Math.Ceiling((double)totalRecords / pageSize),
                    }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<WorkScheduleDetail, bool>> predicate)
        {
            try
            {
                var schedule = await _context.WorkScheduleDetails
                    .Where(predicate)
                    .FirstOrDefaultAsync();

                if (schedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var (response, _) = WorkScheduleDetailConversion.FromEntity(schedule, null);

                return new Response(true, 200, "Thành công")
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

        public async Task<Response> UpdateAsync(DateTime workingDate, Guid workScheduleId, WorkScheduleDetail entity)
        {
            try
            {
                var existingSchedule = await FindByKey(workingDate, workScheduleId, false, true);

                if (existingSchedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                bool hasChanges = existingSchedule.StartTime != entity.StartTime
                    && existingSchedule.EndTime != entity.EndTime
                    && existingSchedule.Note != entity.Note;

                if (!hasChanges)
                {
                    return new Response(false, 204, "Không có thay đổi");
                }

                existingSchedule.StartTime = entity.StartTime;
                existingSchedule.EndTime = entity.EndTime;
                existingSchedule.Note = entity.Note;

                await _context.SaveChangesAsync();

                var (response, _) = WorkScheduleDetailConversion.FromEntity(existingSchedule, null);

                return new Response(true, 200, "Cập nhật thành công")
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

        public async Task<Response> DeleteAsync(DateTime workingDate, Guid workScheduleId)
        {
            try
            {
                var existingSchedule = await FindByKey(workingDate, workScheduleId);

                if (existingSchedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                _context.WorkScheduleDetails.Remove(existingSchedule);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa thành công");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByKeyAsync(DateTime workingDate, Guid workScheduleId)
        {
            try
            {
                var schedule = await FindByKey(workingDate, workScheduleId, true, true);

                if (schedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var (response, _) = WorkScheduleDetailConversion.FromEntity(schedule, null);

                return new Response(true, 200, "Thành công")
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

        public async Task<WorkScheduleDetail> FindByKey(DateTime workingDate, Guid workScheduleId, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.WorkScheduleDetails.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(x => x.WorkSchedule);
            }

            return await query.FirstOrDefaultAsync(x => x.WorkingDate == workingDate && x.WorkScheduleId == workScheduleId) ?? null!;

        }

        // Unused methods
        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, WorkScheduleDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
