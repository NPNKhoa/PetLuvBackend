using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.Interfaces;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceVariantRepository : IServicePrice
    {
        public Task<Response> CreateAsync(IServicePrice entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByAsync(Expression<Func<IServicePrice, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, IServicePrice entity)
        {
            throw new NotImplementedException();
        }
    }
}
