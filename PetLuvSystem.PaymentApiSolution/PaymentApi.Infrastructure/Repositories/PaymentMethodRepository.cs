using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PaymentApi.Infrastructure.Repositories
{
    public class PaymentMethodRepository : IPaymentMethod
    {
        public Task<Response> CreateAsync(PaymentMethod entity)
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

        public Task<Response> GetByAsync(Expression<Func<PaymentMethod, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, PaymentMethod entity)
        {
            throw new NotImplementedException();
        }
    }
}
