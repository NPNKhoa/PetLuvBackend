using Microsoft.EntityFrameworkCore;
using PaymentApi.Application.DTOs.Conversions;
using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PaymentApi.Infrastructure.Data;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PaymentApi.Infrastructure.Repositories
{
    public class PaymentStatusRepository(PaymentDbContext _context, IPaymentCachingService _paymentCachingService) : IPaymentStatus
    {
        public async Task<Response> CreateAsync(PaymentStatus entity)
        {
            try
            {
                var existingEntity = await _context.PaymentStatus.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PaymentStatusName == entity.PaymentStatusName);

                if (existingEntity != null)
                {
                    return new Response(false, 400, "trạng thái thanh toán đã tồn tại");
                }

                await _context.PaymentStatus.AddAsync(entity);
                await _context.SaveChangesAsync();

                await UpdateCache();

                var (response, _) = PaymentStatusConversion.FromEntity(entity, null);

                return new Response(true, 201, "Thêm trạng thái thanh toán thành công")
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
                    return new Response(false, 404, "Không tìm thấy trạng thái thanh toán cần xóa");
                }

                if (existingEntity.IsVisible == true)
                {
                    existingEntity.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Ẩn trạng thái thanh toán thành công");
                }

                if (existingEntity.Payments.Any())
                {
                    return new Response(false, 400, "Không thể xóa trạng thái thanh toán này vì có dữ liệu liên quan");
                }

                _context.PaymentStatus.Remove(existingEntity);
                await _context.SaveChangesAsync();

                await UpdateCache();

                return new Response(true, 200, "Xóa trạng thái thanh toán thành công");
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
                var entities = await _context.PaymentStatus.AsNoTracking()
                    .OrderBy(x => x.PaymentStatusName)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (entities is null || entities.Count == 0)
                {
                    return new Response(false, 404, "Không có dữ liệu trạng thái thanh toán");
                }

                var (_, response) = PaymentStatusConversion.FromEntity(null, entities);

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

        public async Task<Response> GetByAsync(Expression<Func<PaymentStatus, bool>> predicate)
        {
            try
            {
                var entity = await _context.PaymentStatus.AsNoTracking().FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new Response(false, 404, "Không tìm thấy trạng thái thanh toán");
                }

                var (response, _) = PaymentStatusConversion.FromEntity(entity, null);

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
                var existingEntity = await FindById(id, true, true);

                if (existingEntity is null || existingEntity.IsVisible == false)
                {
                    return new Response(false, 404, "trạng thái thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                var (response, _) = PaymentStatusConversion.FromEntity(existingEntity, null);

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

        public async Task<Response> UpdateAsync(Guid id, PaymentStatus entity)
        {
            try
            {
                var existingEntity = await FindById(id, false, true);

                if (existingEntity == null || existingEntity.IsVisible == false)
                {
                    return new Response(false, 404, "trạng thái thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                bool hasChanges = existingEntity.PaymentStatusName != entity.PaymentStatusName
                    || existingEntity.IsVisible != entity.IsVisible;

                if (!hasChanges)
                {
                    return new Response(true, 204, "Không có thay đổi");
                }

                existingEntity.PaymentStatusName = entity.PaymentStatusName;
                existingEntity.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                await UpdateCache();

                var (response, _) = PaymentStatusConversion.FromEntity(existingEntity, null);

                return new Response(true, 200, "Cập nhật trạng thái thanh toán thành công")
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

        public async Task<PaymentStatus> FindById(Guid id, bool noTracking = false, bool includeRelated = false)
        {
            var query = _context.PaymentStatus.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeRelated)
            {
                query = query.Include(x => x.Payments);
            }

            return await query.FirstOrDefaultAsync(x => x.PaymentStatusId == id) ?? null!;
        }

        public async Task<Response> UpdatePaymentStatus(Guid bookingId, Guid statusId)
        {
            try
            {
                var existingPaymentStatus = await _context.Payment.FirstOrDefaultAsync(x => x.OrderId == bookingId);

                if (existingPaymentStatus is null)
                {
                    return new Response(false, 404, "Không tìm thấy payment với booking id");
                }

                if (await _context.PaymentStatus.FindAsync(statusId) is null)
                {
                    return new Response(false, 404, "Không tìm thấy payment status");
                }

                existingPaymentStatus.PaymentStatusId = statusId;

                await _context.SaveChangesAsync();

                return new Response(true, 200, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PaymentStatus> FindByName(string paymentStatusName)
        {
            return await _context.PaymentStatus.FirstOrDefaultAsync(x => x.PaymentStatusName.ToLower().Trim().Equals(paymentStatusName.Trim().ToLower())) ?? null!;
        }

        private async Task UpdateCache()
        {
            var allStatuses = await _context.PaymentStatus.Where(p => p.IsVisible).ToListAsync();
            await _paymentCachingService.UpdateCacheAsync(allStatuses);
        }
    }
}
