using BookingApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace BookingApi.Application.Interfaces
{
    public interface IBookingStatus : IGenericInterface<BookingStatus>
    {
        public Task<BookingStatus> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<Guid> FindBookingStatusIdByName(string bookingStatusName);
    }
}
