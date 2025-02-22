using BookingApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;

namespace BookingApi.Application.Interfaces
{
    public interface IBookingType : IGenericInterface<BookingType>
    {
        public Task<BookingType> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
    }
}
