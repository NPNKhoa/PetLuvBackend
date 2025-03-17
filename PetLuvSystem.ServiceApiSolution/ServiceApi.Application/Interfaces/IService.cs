using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;
using System.Linq.Expressions;

namespace ServiceApi.Application.Interfaces
{
    public interface IService : IGenericInterface<Service>
    {
        public Task<Service> FindServiceById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<Response> GetByAsync(Expression<Func<Service, bool>> predicate, bool isReturnList = false);
    }
}
