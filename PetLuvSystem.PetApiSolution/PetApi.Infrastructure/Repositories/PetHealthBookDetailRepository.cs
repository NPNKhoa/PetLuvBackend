using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Infrastructure.Repositories
{
    public class PetHealthBookDetailRepository : IPetHealthBookDetail
    {
        public Task<Response> CreateAsync(PetHealthBookDetail entity)
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

        public Task<Response> GetByAsync(Expression<Func<PetHealthBookDetail, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, PetHealthBookDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
