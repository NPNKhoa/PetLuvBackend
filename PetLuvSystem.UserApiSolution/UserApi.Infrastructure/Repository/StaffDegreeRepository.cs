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
    public class StaffDegreeRepository(UserDbContext _context) : IStaffDegree
    {
        public async Task<Response> CreateAsync(StaffDegree entity)
        {
            try
            {
                var existingStaffDegree = await _context.StaffDegrees
                    .FirstOrDefaultAsync(x => x.DegreeName.Equals(entity.DegreeName.Trim().ToLower())
                        && x.StaffId == entity.StaffId);

                if (existingStaffDegree is not null)
                {
                    return new Response(false, 409, "Bằng cấp nhân viên đã tồn tại");
                }

                var existingStaff = await _context.Users.FindAsync(entity.StaffId);

                if (existingStaff is null)
                {
                    return new Response(false, 404, "Thông tin nhân viên không hợp lệ");
                }

                if (existingStaff.StaffType.IsNullOrEmpty())
                {
                    return new Response(false, 400, "Bạn không phải nhân viên");
                }

                if (entity.ExpiryDate.ToUniversalTime() <= entity.SignedDate.ToUniversalTime())
                {
                    return new Response(false, 400, "Ngày hết hạn phải sau ngày kí xác nhận");
                }

                await _context.StaffDegrees.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = StaffDegreeConversion.FromEntity(entity, null);

                return new Response(true, 201, "Thêm bằng cấp nhân viên thành công")
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
                var existingStaffDegree = await FindById(id);

                if (existingStaffDegree is null)
                {
                    return new Response(false, 404, "Không tìm thầy bằng cấp");
                }

                if (existingStaffDegree.IsVisible)
                {
                    existingStaffDegree.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Ẩn bằng cấp nhân viên thành công");
                }

                _context.StaffDegrees.Remove(existingStaffDegree);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa bằng cấp nhân viên thành công");
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
                var staffDegrees = await _context.StaffDegrees
                    .Include(x => x.Staff)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (staffDegrees is null || staffDegrees.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thầy bằng cấp");
                }

                var totalRecords = await _context.StaffDegrees.CountAsync();

                var (_, responseData) = StaffDegreeConversion.FromEntity(null, staffDegrees);

                return new Response(true, 200, "Tìm thấy!")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPages = Math.Ceiling((double)totalRecords / pageSize),
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

        public async Task<Response> GetByAsync(Expression<Func<StaffDegree, bool>> predicate)
        {
            try
            {
                var staffDegree = await _context.StaffDegrees
                    .Include(x => x.Staff)
                    .Where(predicate)
                    .FirstOrDefaultAsync();

                if (staffDegree is null)
                {
                    return new Response(false, 404, "Không tìm thầy bằng cấp");
                }

                var (responseData, _) = StaffDegreeConversion.FromEntity(staffDegree, null);

                return new Response(true, 200, "Tìm thấy!")
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
                var staffDegree = await FindById(id, true, true);

                if (staffDegree is null)
                {
                    return new Response(false, 404, "Không tìm thầy bằng cấp");
                }

                var (responseData, _) = StaffDegreeConversion.FromEntity(staffDegree, null);

                return new Response(true, 200, "Staff Degree found")
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

        public async Task<Response> UpdateAsync(Guid id, StaffDegree entity)
        {
            try
            {
                var existingStaffDegree = await FindById(id, false, true);

                if (existingStaffDegree is null)
                {
                    return new Response(false, 404, "Không tìm thầy bằng cấp");
                }

                var existingStaff = await _context.Users.FindAsync(entity.StaffId);

                if (existingStaff is null)
                {
                    return new Response(false, 404, "Thông tin nhân viên không hợp lệ");
                }

                if (existingStaff.StaffType.IsNullOrEmpty())
                {
                    return new Response(false, 400, "Bạn không phải nhân viên");
                }

                if (entity.ExpiryDate.ToUniversalTime() <= entity.SignedDate.ToUniversalTime())
                {
                    return new Response(false, 400, "Ngày hết hạn phải sau ngày kí xác nhận");
                }

                bool hasChanges = !entity.DegreeName.Trim().ToLower().Equals(existingStaffDegree.DegreeName.ToLower())
                    || entity.SignedDate != existingStaffDegree.SignedDate
                    || entity.ExpiryDate != existingStaffDegree.ExpiryDate
                    || entity.DegreeDesc is not null && existingStaffDegree.DegreeDesc is null
                    || entity.DegreeDesc is not null && existingStaffDegree.DegreeDesc is not null
                        && !entity.DegreeDesc.Trim().ToLower().Equals(existingStaffDegree.DegreeDesc.ToLower())
                    || entity.DegreeImage != existingStaffDegree.DegreeImage
                    || entity.IsVisible != existingStaffDegree.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 400, "Không có thay đổi");
                }

                existingStaffDegree.DegreeName = entity.DegreeName;
                existingStaffDegree.SignedDate = entity.SignedDate;
                existingStaffDegree.ExpiryDate = entity.ExpiryDate;
                existingStaffDegree.DegreeDesc = entity.DegreeDesc;
                existingStaffDegree.DegreeImage = entity.DegreeImage;
                existingStaffDegree.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = StaffDegreeConversion.FromEntity(existingStaffDegree, null);

                return new Response(true, 200, "Cập nhật bằng cấp thành công")
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
        public async Task<StaffDegree> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.StaffDegrees.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(x => x.Staff);
            }

            return await query.FirstOrDefaultAsync(x => x.DegreeId == id) ?? null!;
        }
    }
}
