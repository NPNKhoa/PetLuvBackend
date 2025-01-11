using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceRepository : IService
    {
        public Task<Response> CreateAsync(Service entity)
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

        public Task<Response> GetByAsync(Expression<Func<Service, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, Service entity)
        {
            throw new NotImplementedException();
        }
    }
}
