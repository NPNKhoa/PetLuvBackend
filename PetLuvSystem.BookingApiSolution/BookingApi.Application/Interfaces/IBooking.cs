using BookingApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace BookingApi.Application.Interfaces
{
    public interface IBooking : IGenericInterface<Booking>
    {
        public Task<Booking> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
