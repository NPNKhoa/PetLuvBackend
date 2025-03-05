using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace PaymentApi.Application.Interfaces
{
    public interface IPaymentStatus : IGenericInterface<PaymentStatus>
    {
        public Task<PaymentStatus> FindById(Guid id, bool noTracking = false, bool includeRelated = false);
        public Task<PaymentStatus> FindByName(string paymentStatusName);
        public Task<Response> UpdatePaymentStatus(Guid bookingId, Guid statusId);
    }
}
