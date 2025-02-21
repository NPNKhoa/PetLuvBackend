using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentApi.Application.Interfaces;
using PaymentApi.Infrastructure.Data;
using PaymentApi.Infrastructure.Repositories;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;

namespace PaymentApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<PaymentDbContext>(services, config);

            CloudinaryHelper.ConfigureCloudinary(config);

            services.AddScoped<IPayment, PaymentRepository>();
            services.AddScoped<IPaymentHistory, PaymentHistoryRepository>();
            services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
            services.AddScoped<IPaymentStatus, PaymentStatusRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }

    }
}
