using PetLuvSystem.SharedLibrary.Interfaces;
using UserApi.Domain.Etities;

namespace UserApi.Application.Interfaces
{
    public interface IWorkSchedule : IGenericInterface<WorkSchedule>
    {
        public Task<WorkSchedule> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
