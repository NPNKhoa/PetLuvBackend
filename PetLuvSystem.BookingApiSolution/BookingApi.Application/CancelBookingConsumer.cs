using BookingApi.Application.Interfaces;
using MassTransit;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace BookingApi.Application
{
    public class CancelBookingConsumer : IConsumer<CancelBookingCommand>
    {
        private readonly IBooking _booking;
        private readonly IBookingStatus _bookingStatus;

        public CancelBookingConsumer(IBooking booking, IBookingStatus bookingStatus)
        {
            _booking = booking;
            _bookingStatus = bookingStatus;

            LogException.LogInformation("[Payment Service] CreateaPaymentConsumer initialized");
        }

        public async Task Consume(ConsumeContext<CancelBookingCommand> context)
        {
            var command = context.Message;

            LogException.LogInformation($"[Booking Service] Processing Cancel Booking Message for BookingId {command.BookingId}");

            var bookingStatusId = await _bookingStatus.FindBookingStatusIdByName("Đã hủy");

            if (bookingStatusId == Guid.Empty)
            {
                LogException.LogError($"[Booking Service] Can't not find any booking status with this name");
                return;
            }

            var exstingBoooking = await _booking.FindById(command.BookingId, false, false);
            exstingBoooking.BookingStatusId = bookingStatusId;

            var response = await _booking.UpdateAsync(command.BookingId, exstingBoooking);

            if (response.Flag == false)
            {
                LogException.LogError($"[Booking Service] Failed to update booking status. Message: {response.Message}");
                return;
            }
        }
    }
}
