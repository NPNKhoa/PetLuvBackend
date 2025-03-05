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

            services.AddHttpClient();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            services.AddScoped<ICheckPetService, CheckPetService>();
            services.AddScoped<ICheckCustomerService, CheckCustomerService>();
            services.AddScoped<ICheckPaymentStatusService, CheckPaymentStatusService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IServiceService, ServiceService>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
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
