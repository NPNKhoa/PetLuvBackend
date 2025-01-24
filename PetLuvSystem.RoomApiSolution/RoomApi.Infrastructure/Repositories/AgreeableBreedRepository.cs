using PetLuvSystem.SharedLibrary.Responses;
using RoomApi.Application.Interfaces;
using RoomApi.Domain.Entities;
using RoomApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace RoomApi.Infrastructure.Repositories
{
    public class AgreeableBreedRepository(RoomDbContext _context) : IAgreeableBreed
    {
        public Task<Response> CreateAsync(AgreeableBreed entity)
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

        public Task<Response> GetByAsync(Expression<Func<AgreeableBreed, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, AgreeableBreed entity)
        {
            throw new NotImplementedException();
        }
    }
}
