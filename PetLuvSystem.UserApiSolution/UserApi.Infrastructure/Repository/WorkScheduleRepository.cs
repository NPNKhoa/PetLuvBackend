using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.Interfaces;
using UserApi.Domain.Etities;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repository
{
    public class WorkScheduleRepository(UserDbContext _context) : IWorkSchedule
    {
        public async Task<Response> CreateAsync(WorkSchedule entity)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == entity.StaffId);

                if (existingUser is null)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng này");
                }

                if (existingUser.StaffType.IsNullOrEmpty())
                {
                    return new Response(false, 400, "Người dùng này không phải nhân viên");
                }

                var existingSchedule = await _context.WorkSchedules
                    .Where(x => x.StaffId == entity.StaffId)
                    .FirstOrDefaultAsync();

                if (existingSchedule is not null)
                {
                    return new Response(false, 409, "Nhân viên này đã có lịch làm rồi");
                }

                await _context.WorkSchedules.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = WorkScheduleConversion.FromEntity(entity, null);

                return new Response(true, 201, "Tạo thành công")
                {
                    Data = responseData
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
                var existingSchedule = await FindById(id);

                if (existingSchedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                if (existingSchedule.WorkScheduleDetails.Count > 0)
                {
                    return new Response(false, 409, "Không thể xóa lịch làm này vì đã có chi tiết lịch làm");
                }

                _context.WorkSchedules.Remove(existingSchedule);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa thành công");
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
                var workSchedules = await _context.WorkSchedules
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(ws => ws.Staff)
                    .ToListAsync();

                if (workSchedules is null || workSchedules.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var (_, responseData) = WorkScheduleConversion.FromEntity(null, workSchedules);

                return new Response(true, 200, "Lấy dữ liệu thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<WorkSchedule, bool>> predicate)
        {
            try
            {
                var workSchedule = await _context.WorkSchedules
                    .Where(predicate)
                    .Include(x => x.Staff)
                    .Include(x => x.WorkScheduleDetails)
                    .FirstOrDefaultAsync();

                if (workSchedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var (responseData, _) = WorkScheduleConversion.FromEntity(workSchedule, null);

                return new Response(true, 200, "Lấy dữ liệu thành công")
                {
                    Data = responseData
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
                var workSchedule = await FindById(id, true, true);

                if (workSchedule is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch làm");
                }

                var (responseData, _) = WorkScheduleConversion.FromEntity(workSchedule, null);

                return new Response(true, 200, "Lấy dữ liệu thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<WorkSchedule> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.WorkSchedules.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(x => x.Staff).Include(x => x.WorkScheduleDetails);
            }

            return await query.FirstOrDefaultAsync(x => x.WorkScheduleId == id) ?? null!;
        }

        // Unimplemented methods
        public Task<Response> UpdateAsync(Guid id, WorkSchedule entity)
        {
            throw new NotImplementedException();
        }
    }
}
