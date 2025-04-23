using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace PaymentApi.Application.Interfaces
{
    public interface IPayment : IGenericInterface<Payment>
    {
        public Task<Response> CreatePaymentUrlAsync(Guid bookingId, Guid userId, decimal money, string description, string ipAddress);
        public Task<Payment> FindById(Guid id, bool noTacking = false, bool includeOthers = false);
        public Task<Payment> FindByRef(string transactionRef);
        public Task<Response> GetByUser(Guid userId);
        public Task<Response> UpdateStatus(Guid orderId, decimal amount, bool isComplete);
    }
}
