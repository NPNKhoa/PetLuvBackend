using BookingApi.Application.Interfaces;
using BookingApi.Infrastructure.Data;
using BookingApi.Infrastructure.Repositories;
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

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }

    }
}
