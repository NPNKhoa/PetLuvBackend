using PetLuvSystem.SharedLibrary.Interfaces;
using UserApi.Domain.Etities;

namespace UserApi.Application.Interfaces
{
    public interface IStaffDegree : IGenericInterface<StaffDegree>
    {
        public Task<StaffDegree> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
