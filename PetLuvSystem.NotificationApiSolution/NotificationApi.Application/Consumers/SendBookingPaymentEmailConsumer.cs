using MassTransit;
using NotificationApi.Application.DTOs;
using NotificationApi.Application.Interfaces;
using PetLuvSystem.SharedLibrary.Contracts.Events;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace NotificationApi.Application.Consumers
{
    public class SendBookingPaymentEmailConsumer : IConsumer<SendBookingPaymentEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IPublishEndpoint _publishEndpoint;

        public SendBookingPaymentEmailConsumer(IEmailService emailService, IPublishEndpoint publishEndpoint)
        {
            _emailService = emailService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SendBookingPaymentEmailCommand> context)
        {
            var command = context.Message;

            LogException.LogInformation($"[Notification service] Received request to send email to {command.CustomerEmail} for Booking {command.BookingId} with Payment URL {command.PaymentUrl}");

            var emailRequest = new SendEmailRequestDTO
            {
                ToEmail = command.CustomerEmail,
                Subject = "Thanh toán xác nhận lịch hẹn",
                BookingId = command.BookingId,
                PaymentUrl = command.PaymentUrl
            };

            if (string.IsNullOrWhiteSpace(emailRequest.ToEmail))
            {
                LogException.LogError($"[Notification Service] Customer email is null for Booking {command.BookingId}");
                return;
            }

            var emailSent = await _emailService.SendEmailAsync(emailRequest);

            if (!emailSent)
            {
                LogException.LogError($"[Notification Service] Fail to Send email for booking with ID {command.BookingId}");
                return;
            }

            await _publishEndpoint.Publish(
                new BookingPaymentEmailSentEvent
                {
                    BookingId = command.BookingId,
                    CustomerId = command.CustomerId,
                    PaymentUrl = command.PaymentUrl,
                    SentAt = DateTime.UtcNow
                }
            );

            LogException.LogInformation($"Email sent succesfully with subject {emailRequest.Subject} for Booking {emailRequest.BookingId}");
        }
    }
}
