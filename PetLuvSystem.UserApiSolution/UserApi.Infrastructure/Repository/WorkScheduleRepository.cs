using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;
using UserApi.Application.Interfaces;
using UserApi.Domain.Etities;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repository
{
    public class WorkScheduleRepository(UserDbContext _context) : IWorkSchedule
    {
        public Task<Response> CreateAsync(WorkSchedule entity)
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

        public Task<Response> GetByAsync(Expression<Func<WorkSchedule, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, WorkSchedule entity)
        {
            throw new NotImplementedException();
        }
    }
}
