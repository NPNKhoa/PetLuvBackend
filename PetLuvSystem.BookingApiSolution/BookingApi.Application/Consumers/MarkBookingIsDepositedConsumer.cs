using BookingApi.Application.Interfaces;
using MassTransit;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace BookingApi.Application.Consumers
{
    public class MarkBookingIsDepositedConsumer : IConsumer<MarkBookingIsDepositedCommand>
    {
        private readonly IBooking _bookingRepo;
        private readonly IBookingStatus _bookingStatusRepo;

        public MarkBookingIsDepositedConsumer(IBooking bookingRepo, IBookingStatus bookingStatusRepo)
        {
            _bookingRepo = bookingRepo;
            _bookingStatusRepo = bookingStatusRepo;
        }

        public async Task Consume(ConsumeContext<MarkBookingIsDepositedCommand> context)
        {
            try
            {
                var command = context.Message;

                LogException.LogInformation($"[Booking] Consuming change booking's payment status...");

                var booking = await _bookingRepo.FindById(command.BookingId, false, true);

                if (booking is null)
                {
                    LogException.LogInformation($"[Booking] Fail to find booking with id {command.BookingId}");
                    return;
                }

                var bookingStatusId = await _bookingStatusRepo.FindBookingStatusIdByName(command.IsSuccess ? "Đang xử lý" : "Đã hủy");

                if (bookingStatusId == Guid.Empty)
                {
                    LogException.LogInformation($"[Booking] Fail to find Booking Status");
                    return;
                }

                booking.BookingStatusId = bookingStatusId;
                booking.PaymentStatusId = command.PaymentStatusId;
                booking.DepositAmount = command.DepositAmount;

                LogException.LogInformation($"[Booking] Payment is {(command.IsSuccess ? "success" : "failed")}");

                await _bookingRepo.UpdateAsync(command.BookingId, booking);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
            }
        }
    }
}
