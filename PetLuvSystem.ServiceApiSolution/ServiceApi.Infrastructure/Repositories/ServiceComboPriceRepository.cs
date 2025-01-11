using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.Interfaces;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceComboPriceRepository : IServiceComboPrice
    {
        public Task<Response> CreateAsync(IServiceComboPrice entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByAsync(Expression<Func<IServiceComboPrice, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, IServiceComboPrice entity)
        {
            throw new NotImplementedException();
        }
    }
}
