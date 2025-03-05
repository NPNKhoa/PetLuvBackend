using BookingOrchestrator.Application.Interfaces;
using BookingOrchestrator.Domain.Entities;
using MongoDB.Driver;

namespace BooingOrchestrator.Infrastructure.Repositories
{
    public class BookingStateRepository : IBookingStateRepository
    {
        private readonly IMongoCollection<BookingState> _collection;

        public BookingStateRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<BookingState>("BookingStates");
        }

        public async Task CreateAsync(BookingState bookingState)
        {
            await _collection.InsertOneAsync(bookingState);
        }

        public async Task<BookingState> GetByBookingIdAsync(Guid bookingId)
        {
            return await _collection.Find(b => b.BookingId == bookingId).FirstOrDefaultAsync();
        }

        public async Task<BookingState> GetByCorrelationIdAsync(Guid correlationId)
        {
            return await _collection.Find(b => b.CorrelationId == correlationId).FirstOrDefaultAsync();
        }

        public async Task UpdateStatusAsync(Guid bookingId, string status)
        {
            var filter = Builders<BookingState>.Filter.Eq(b => b.BookingId, bookingId);
            var update = Builders<BookingState>.Update.Set(b => b.Status, status);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
