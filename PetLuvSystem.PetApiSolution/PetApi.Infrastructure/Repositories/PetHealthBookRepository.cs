using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Infrastructure.Repositories
{
    public class PetHealthBookRepository : IPetHealthBook
    {
        public Task<Response> CreateAsync(PetHealthBook entity)
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

        public Task<Response> GetByAsync(Expression<Func<PetHealthBook, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, PetHealthBook entity)
        {
            throw new NotImplementedException();
        }
    }
}
