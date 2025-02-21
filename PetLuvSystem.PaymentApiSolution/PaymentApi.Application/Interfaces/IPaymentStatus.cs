using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PaymentApi.Application.Interfaces
{
    public interface IPaymentStatus : IGenericInterface<PaymentStatus>
    {
        public Task<PaymentStatus> FindById(Guid id, bool noTracking = false, bool includeRelated = false);
    }
}
