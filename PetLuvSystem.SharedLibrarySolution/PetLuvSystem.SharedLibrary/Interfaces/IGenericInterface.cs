using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetLuvSystem.SharedLibrary.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
