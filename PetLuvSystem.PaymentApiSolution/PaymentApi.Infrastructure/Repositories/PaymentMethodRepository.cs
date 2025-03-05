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
    public class PaymentMethodRepository(PaymentDbContext _context) : IPaymentMethod
    {
        public async Task<Response> CreateAsync(PaymentMethod entity)
        {
            var existingEntity = await _context.PaymentMethod.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PaymentMethodName == entity.PaymentMethodName);

            if (existingEntity != null)
            {
                return new Response(false, 400, "phương thức thanh toán đã tồn tại");
            }

            await _context.PaymentMethod.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new Response(true, 201, "Thêm phương thức thanh toán thành công");
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var existingEntity = await FindById(id, false, true);

                if (existingEntity is null)
                {
                    return new Response(false, 404, "Không tìm thấy phương thức thanh toán cần xóa");
                }

                if (existingEntity.IsVisible == true)
                {
                    existingEntity.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Ẩn phương thức thanh toán thành công");
                }

                if (existingEntity.Payments.Any())
                {
                    return new Response(false, 400, "Không thể xóa phương thức thanh toán này vì có dữ liệu liên quan");
                }

                _context.PaymentMethod.Remove(existingEntity);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa phương thức thanh toán thành công");
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
                var entities = await _context.PaymentMethod
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (entities is null || !entities.Any())
                {
                    return new Response(false, 404, "Không tìm thấy phương thức thanh toán nào");
                }

                var (_, response) = PaymentMethodConversion.FromEntity(null, entities);

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

        public async Task<Response> GetByAsync(Expression<Func<PaymentMethod, bool>> predicate)
        {
            try
            {
                var entity = await _context.PaymentMethod.AsNoTracking().FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new Response(false, 404, "Không tìm thấy phương thức thanh toán");
                }

                var (response, _) = PaymentMethodConversion.FromEntity(entity, null);

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
                    return new Response(false, 404, "Phương thức thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                var (response, _) = PaymentMethodConversion.FromEntity(existingEntity, null);

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

        public async Task<Response> UpdateAsync(Guid id, PaymentMethod entity)
        {
            try
            {
                var existingEntity = await FindById(id, false, true);

                if (existingEntity == null || existingEntity.IsVisible == false)
                {
                    return new Response(false, 404, "phương thức thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                bool hasChanges = existingEntity.PaymentMethodName != entity.PaymentMethodName
                    || existingEntity.IsVisible != entity.IsVisible;

                if (!hasChanges)
                {
                    return new Response(true, 204, "Không có thay đổi");
                }

                existingEntity.PaymentMethodName = entity.PaymentMethodName;
                existingEntity.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (response, _) = PaymentMethodConversion.FromEntity(existingEntity, null);

                return new Response(true, 200, "Cập nhật phương thức thanh toán thành công")
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

        public async Task<PaymentMethod> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.PaymentMethod.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(pm => pm.Payments);
            }

            return await query.FirstOrDefaultAsync(pm => pm.PaymentMethodId == id) ?? null!;
        }

        public async Task<PaymentMethod> FindByName(string paymentMethodName)
        {
            return await _context.PaymentMethod.FirstOrDefaultAsync(x => x.PaymentMethodName.ToLower().Trim().Equals(paymentMethodName.Trim().ToLower())) ?? null!;
        }
    }
}
