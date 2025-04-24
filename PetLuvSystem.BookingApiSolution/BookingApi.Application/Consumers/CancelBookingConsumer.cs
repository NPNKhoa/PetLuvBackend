using BookingApi.Application.Interfaces;
using MassTransit;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace BookingApi.Application.Consumers
{
    public class CancelBookingConsumer : IConsumer<CancelBookingCommand>
    {
        private readonly IBooking _booking;

        public CancelBookingConsumer(IBooking booking)
        {
            _booking = booking;
        }

        public async Task Consume(ConsumeContext<CancelBookingCommand> context)
        {
            LogException.LogInformation("[Booking] Starting Rollback...!");
            try
            {
                var message = context.Message;

                var response = await _booking.DeleteAsync(message.BookingId);

                if (!response.Flag)
                {
                    LogException.LogInformation($"[Booking] An error occur while processing CancelBookingCommand. {response.Message}");
                }

                LogException.LogInformation("[Booking] Rollback successfully!");
            }
            catch (Exception ex)
            {
                LogException.LogInformation("[Booking] An error occur while processing CancelBookingCommand");

                if (ex.InnerException is not null)
                {
                    LogException.LogExceptions(ex.InnerException);
                }

                LogException.LogExceptions(ex);
            }
        }
    }
}
