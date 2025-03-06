using BookingOrchestrator.Domain.Entities;
using MassTransit;
using PetLuvSystem.SharedLibrary.Contracts.Events;
using PetLuvSystem.SharedLibrary.Contracts.Messages;
using PetLuvSystem.SharedLibrary.Logs;

namespace BookingOrchestrator.Application.Saga
{
    public class BookingSagaStateMachine : MassTransitStateMachine<BookingState>
    {
        // States
        public State AwaitingPayment { get; set; }
        public State AwaitingPaymentConfirmation { get; set; }
        public State Completed { get; set; }
        public State Canceled { get; set; }

        // Events
        public Event<BookingCreatedEvent> BookingCreated { get; private set; }
        public Event<PaymentCreatedEvent> PaymentCreated { get; private set; }
        public Event<PaymentCompletedEvent> PaymentCompleted { get; private set; }
        public Event<PaymentRejectedEvent> PaymentRejected { get; private set; }
        public Event<PaymentSystemErrorEvent> PaymentSystemError { get; private set; }
        public Event<BookingConfirmationEmailSentEvent> BookingConfirmationEmailSent { get; private set; }
        public Event<BookingConfirmationEmailFailedEvent> BookingConfirmationEmailFailed { get; private set; }

        public BookingSagaStateMachine()
        {
            InstanceState(x => x.Status);

            Event(() => BookingCreated, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => PaymentCreated, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => PaymentRejected, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => PaymentSystemError, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => BookingConfirmationEmailSent, x => x.CorrelateById(m => m.Message.BookingId));
            Event(() => BookingConfirmationEmailFailed, x => x.CorrelateById(m => m.Message.BookingId));

            Initially(
                When(BookingCreated)
                    .Then(context =>
                    {
                        Console.WriteLine($"[Orchestrator] Received BookingCreated event with Email: {context.Message.CustomerEmail}");
                        context.Saga.BookingId = context.Message.BookingId;
                        context.Saga.CustomerId = context.Message.CustomerId;
                        context.Saga.CustomerEmail = context.Message.CustomerEmail;
                        context.Saga.TotalPrice = context.Message.TotalPrice;
                        context.Saga.CreatedAt = DateTime.UtcNow;
                        context.Saga.Status = "AwaitingPayment";
                    })
                    .SendAsync(new Uri("queue:payment-service"), context =>
                        Task.FromResult(new CreateaPaymentCommand
                        {
                            BookingId = context.Message.BookingId,
                            CustomerId = context.Message.CustomerId,
                            Amount = context.Message.TotalPrice,
                            Description = context.Message.Description,
                            IpAddress = context.Message.IpAddress
                        })
                    )
                    .TransitionTo(AwaitingPayment)
            );

            During(AwaitingPayment,
                When(PaymentCreated)
                    .Then(context =>
                    {
                        Console.WriteLine($"[Orchestrator] Received PaymentCreatedEvent for Booking {context.Message.BookingId} " +
                            $"with payment url {context.Message.PaymentUrl} and Customer email {context.Saga.CustomerEmail}");
                        context.Saga.PaymentUrl = context.Message.PaymentUrl;
                        context.Saga.Status = "AwaitingPaymentConfirmation";
                    })
                    .SendAsync(new Uri("queue:notification-service"), context =>
                        Task.FromResult(new SendBookingPaymentEmailCommand
                        {
                            BookingId = context.Message.BookingId,
                            CustomerId = context.Saga.CustomerId,
                            CustomerEmail = context.Saga.CustomerEmail,
                            PaymentUrl = context.Message.PaymentUrl
                        })
                    )
                    .TransitionTo(AwaitingPaymentConfirmation)
            );

            During(AwaitingPaymentConfirmation,
                When(PaymentCompleted)
                    .Then(context =>
                    {
                        context.Saga.PaymentCompletedAt = DateTime.UtcNow;
                        context.Saga.Status = "Completed";
                    })
                    .SendAsync(new Uri("queue:notification-service"), context =>
                        Task.FromResult(new SendBookingConfirmationEmailCommand
                        {
                            BookingId = context.Message.BookingId,
                            CustomerId = context.Saga.CustomerId,
                            AmountPaid = context.Saga.TotalPrice
                        })
                    )
                    .TransitionTo(Completed),

                When(PaymentRejected)
                    .Then(context =>
                    {
                        context.Saga.Status = "Payment Rejected";
                        context.Saga.CanceledAt = DateTime.UtcNow;
                    })
                    .SendAsync(new Uri("queue:booking-service"), context =>
                        Task.FromResult(new CancelBookingCommand
                        {
                            BookingId = context.Message.BookingId,
                            Reason = context.Message.Reason
                        })
                    )
                    .TransitionTo(Canceled),

                When(PaymentSystemError)
                    .Then(context =>
                    {
                        context.Saga.CanceledAt = DateTime.UtcNow;
                        context.Saga.Status = "Payment System Error";
                    })
                    .TransitionTo(Canceled)
            );

            During(Completed,
                When(BookingConfirmationEmailSent)
                    .Then(context =>
                    {
                        context.Saga.EmailSentAt = DateTime.UtcNow;
                    })
                    .Finalize()
            );

            During(Canceled,
                When(BookingConfirmationEmailFailed)
                    .Then(context =>
                    {
                        context.Saga.Status = "Canceled";
                        LogException.LogError("Fail to Send Email");
                    })
            );

            SetCompletedWhenFinalized();
        }
    }
}
