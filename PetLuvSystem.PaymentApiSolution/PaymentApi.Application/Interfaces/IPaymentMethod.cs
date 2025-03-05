using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace PaymentApi.Application.Interfaces
{
    public interface IPaymentMethod : IGenericInterface<PaymentMethod>
    {
        public Task<PaymentMethod> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<PaymentMethod> FindByName(string paymentMethodName);
    }
}
