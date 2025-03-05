using BookingOrchestrator.Domain.Entities;

namespace BookingOrchestrator.Application.Interfaces
{
    public interface IBookingStateRepository
    {
        Task CreateAsync(BookingState bookingState);
        Task<BookingState> GetByBookingIdAsync(Guid bookingId);
        Task<BookingState> GetByCorrelationIdAsync(Guid correlationId);
        Task UpdateStatusAsync(Guid bookingId, string status);
    }

}
