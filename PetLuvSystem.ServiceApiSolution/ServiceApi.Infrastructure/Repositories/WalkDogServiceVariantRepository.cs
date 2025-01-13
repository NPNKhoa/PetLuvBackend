using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.Interfaces;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class WalkDogServiceVariantRepository : IWalkDogServicePrice
    {
        public Task<Response> CreateAsync(IWalkDogServicePrice entity)
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

        public Task<Response> GetByAsync(Expression<Func<IWalkDogServicePrice, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, IWalkDogServicePrice entity)
        {
            throw new NotImplementedException();
        }
    }
}
