using BookingApi.Application.Consumers;
using BookingApi.Application.Interfaces;
using BookingApi.Infrastructure.Data;
using BookingApi.Infrastructure.Repositories;
using BookingApi.Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;

namespace BookingApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<BookingDbContext>(services, config);

            services.AddScoped<IBooking, BookingRepository>();
            services.AddScoped<IBookingStatus, BookingStatusRepository>();
            services.AddScoped<IBookingType, BookingTypeRepository>();

            services.AddScoped<IStatistic, StatisticRepository>();

            services.AddHttpClient();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            services.AddScoped<ICheckPetService, CheckPetService>();
            services.AddScoped<ICheckCustomerService, CheckCustomerService>();
            services.AddScoped<ICheckPaymentStatusService, CheckPaymentStatusService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IBreedMappingService, BreedMappingService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<MarkBookingIsDepositedConsumer>();
                x.AddConsumer<CancelBookingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("booking-service", e =>
                    {
                        e.ConfigureConsumer<MarkBookingIsDepositedConsumer>(context);
                        e.ConfigureConsumer<CancelBookingConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }

    }
}
