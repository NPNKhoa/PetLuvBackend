using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Application.Interfaces
{
    public interface IPetHealthBookDetail : IGenericInterface<PetHealthBookDetail>
    {
        public Task<PetHealthBookDetail> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<Response> GetByHealthBook(Guid id, int pageIndex = 1, int pageSize = 10);
    }
}
