using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using UserApi.Domain.Etities;

namespace UserApi.Application.Interfaces
{
    public interface IWorkScheduleDetail : IGenericInterface<WorkScheduleDetail>
    {
        public Task<WorkScheduleDetail> FindByKey(DateTime workingDate, Guid workScheduleId, bool noTracking = false, bool includeOthers = false);
        public Task<Response> UpdateAsync(DateTime workingDate, Guid workScheduleId, WorkScheduleDetail entity);
        public Task<Response> DeleteAsync(DateTime workingDate, Guid workScheduleId);
        public Task<Response> GetByKeyAsync(DateTime workingDate, Guid workScheduleId);
    }
}
