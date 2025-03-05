using MassTransit;
using PaymentApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Contracts.Events;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace PaymentApi.Application.Consumers
{
    public class CreateaPaymentConsumer : IConsumer<CreateaPaymentCommand>
    {
        private readonly IPayment _paymentRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateaPaymentConsumer(IPayment paymentRepository, IPublishEndpoint publishEndpoint)
        {
            _paymentRepository = paymentRepository;
            _publishEndpoint = publishEndpoint;

            LogException.LogInformation("[Payment Service] CreateaPaymentConsumer initialized");
        }

        public async Task Consume(ConsumeContext<CreateaPaymentCommand> context)
        {
            var command = context.Message;

            LogException.LogInformation($"Số tiến thanh toán: {command.Amount}");

            LogException.LogInformation($"[Payment Service] Processing payment for booking {command.BookingId}");

            var paymentResponse = await _paymentRepository.CreatePaymentUrlAsync(
                command.BookingId,
                command.CustomerId,
                command.Amount,
                command.Description,
                command.IpAddress
            );

            if (!paymentResponse.Flag)
            {
                LogException.LogError($"[Payment Service] Fail to create payment url for booking {command.BookingId}");
                return;
            }

            string paymentUrl = paymentResponse.Data?.ToString() ?? string.Empty;

            if (paymentUrl == string.Empty)
            {
                LogException.LogError($"[Payment Service] Data from Payment Response is return null");
                return;
            }

            await _publishEndpoint.Publish(new PaymentCreatedEvent
            {
                BookingId = command.BookingId,
                PaymentUrl = paymentUrl
            });

            Console.WriteLine($"[Payment Service] Payment URL created: {paymentUrl}");
        }
    }
}
