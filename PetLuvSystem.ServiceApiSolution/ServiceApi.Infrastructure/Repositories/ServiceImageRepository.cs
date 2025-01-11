using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.Interfaces;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceImageRepository : IServiceImage
    {
        public Task<Response> CreateAsync(IServiceImage entity)
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

        public Task<Response> GetByAsync(Expression<Func<IServiceImage, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, IServiceImage entity)
        {
            throw new NotImplementedException();
        }
    }
}
