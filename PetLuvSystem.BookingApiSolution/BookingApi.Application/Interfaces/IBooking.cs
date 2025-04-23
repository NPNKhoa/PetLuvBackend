using BookingApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace BookingApi.Application.Interfaces
{
    public interface IBooking : IGenericInterface<Booking>
    {
        public Task<Booking> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<Response> ValidateBookingCreation(Booking entity);
    }
}
