using PaymentApi.Domain.Entities;

namespace PaymentApi.Application.Interfaces
{
    public interface IPaymentCachingService
    {
        public Task UpdateCacheAsync(List<PaymentStatus> paymentStatuses);
    }
}
