using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetLuvSystem.SharedLibrary.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(Guid id, T entity);
        Task<Response> DeleteAsync(Guid id);
        Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10);
        Task<Response> GetByIdAsync(Guid id);
        Task<Response> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
