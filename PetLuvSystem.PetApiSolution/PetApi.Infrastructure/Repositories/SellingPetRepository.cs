using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Infrastructure.Repositories
{
    public class SellingPetRepository : ISellingPet
    {
        public Task<Response> CreateAsync(SellingPet entity)
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

        public Task<Response> GetByAsync(Expression<Func<SellingPet, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, SellingPet entity)
        {
            throw new NotImplementedException();
        }
    }
}
