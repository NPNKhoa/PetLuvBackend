using Microsoft.EntityFrameworkCore;
using PaymentApi.Application.DTOs.Conversions;
using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PaymentApi.Infrastructure.Data;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;

namespace PaymentApi.Infrastructure.Repositories
{
    public class PaymentRepository(PaymentDbContext _context,
        IVnpay _vnpay, IPaymentStatus _paymentStatus, IPaymentMethod _paymentMethod) : IPayment
    {
        public async Task<Response> CreateAsync(Payment entity)
        {
            try
            {
                var existingEntity = await _context.Payment.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.TransactionRef == entity.TransactionRef);

                if (existingEntity != null)
                {
                    return new Response(false, 400, "TransactionRef đã tồn tại");
                }

                await _context.Payment.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new Response(true, 201, "Thêm thanh toán thành công");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex.InnerException is not null)
                {
                    LogException.LogError(ex.InnerException.Message);
                }

                return (new Response(false, 5000, "INternal Server Error"));
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var existingEntity = await FindById(id, false, true);

                if (existingEntity is null)
                {
                    return new Response(false, 404, "Không tìm thấy lịch sử thanh toán");
                }

                _context.Payment.Remove(existingEntity);
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
                var entities = await _context.Payment
                    .Include(p => p.PaymentMethod)
                    .Include(p => p.PaymentStatus)
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (entities is null || entities.Count == 0)
                {
                    return new Response(false, 404, "Không có dữ liệu trạng thái thanh toán");
                }

                var (_, response) = PaymentConversion.FromEntity(null, entities);

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

        public async Task<Response> GetByUser(Guid userId)
        {
            try
            {
                var entities = await _context.Payment.AsNoTracking()
                    .Include(p => p.PaymentStatus)
                    .Include(p => p.PaymentMethod)
                    .Where(p => p.UserId == userId && !p.PaymentStatus.PaymentStatusName.ToLower().Contains("chờ"))
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                if (entities is null || entities.Count == 0)
                {
                    return new Response(false, 404, "Không có dữ liệu lịch sử thanh toán");
                }

                var (_, response) = PaymentConversion.FromEntity(null, entities);

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

        public async Task<Response> GetByAsync(Expression<Func<Payment, bool>> predicate)
        {
            try
            {
                var entity = await _context.Payment.AsNoTracking().FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new Response(false, 404, "Không tìm thấy trạng thái thanh toán");
                }

                var (response, _) = PaymentConversion.FromEntity(entity, null);

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

                if (existingEntity is null)
                {
                    return new Response(false, 404, "Lịch sử thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                var (response, _) = PaymentConversion.FromEntity(existingEntity, null);

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

        public async Task<Response> UpdateAsync(Guid id, Payment entity)
        {
            try
            {
                LogException.LogInformation($"[Payment] Đang Cập nhật");
                var existingEntity = await FindById(id, false, true);

                if (existingEntity == null)
                {
                    return new Response(false, 404, "Lịch sử thanh toán cần tìm không tồn tại hoặc bị xóa");
                }

                existingEntity.PaymentStatusId = entity.PaymentStatusId;

                await _context.SaveChangesAsync();

                LogException.LogInformation($"[Payment] Cập nhật thành công");

                var (response, _) = PaymentConversion.FromEntity(existingEntity, null);

                return new Response(true, 200, "Cập nhật lịch sử thanh toán thành công")
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

        public async Task<Response> CreatePaymentUrlAsync(Guid bookingId, Guid userId, decimal money, string description, string ipAddress)
        {
            try
            {
                var transactionRef = DateTime.UtcNow.Ticks.ToString();

                var request = new PaymentRequest
                {
                    PaymentId = long.Parse(transactionRef),
                    Money = (double)money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY,
                    CreatedDate = DateTime.Now,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                var paymentStatus = await _paymentStatus.FindByName("Chờ thanh toán");
                if (paymentStatus is null)
                {
                    return new Response(false, 404, "Không tìm thấy trạng thái thanh toán");
                }

                var paymentMethod = await _paymentMethod.FindByName("Thanh toán qua VNPay");
                if (paymentMethod is null)
                {
                    return new Response(false, 404, "Không tìm thấy trạng thái thanh toán");
                }

                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    Amount = (decimal)money,
                    TransactionRef = transactionRef,
                    OrderId = bookingId,
                    UserId = userId,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    PaymentStatusId = paymentStatus.PaymentStatusId,
                };

                await _context.Payment.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new Response(true, 201, "Tạo payment URL thành công")
                {
                    Data = paymentUrl
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Payment> FindById(Guid id, bool noTacking = false, bool includeOthers = false)
        {
            var query = _context.Payment.AsQueryable();

            if (noTacking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(p => p.PaymentMethod).Include(p => p.PaymentStatus);
            }

            return await query.FirstOrDefaultAsync(p => p.PaymentId == id) ?? null!;
        }

        public async Task<Payment> FindByRef(string transactionRef)
        {
            return await _context.Payment.Include(p => p.PaymentStatus).FirstOrDefaultAsync(x => x.TransactionRef.Equals(transactionRef)) ?? null!;
        }
    }
}
