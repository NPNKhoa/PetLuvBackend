using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PaymentApi.Infrastructure.Repositories
{
    public class PaymentHistoryRepository : IPaymentHistory
    {
        public Task<Response> CreateAsync(PaymentHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByAsync(Expression<Func<PaymentHistory, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, PaymentHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}
