using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentApi.Application.Consumers;
using PaymentApi.Application.Interfaces;
using PaymentApi.Infrastructure.Data;
using PaymentApi.Infrastructure.Repositories;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;
using VNPAY.NET;

namespace PaymentApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<PaymentDbContext>(services, config);

            CloudinaryHelper.ConfigureCloudinary(config);

            services.AddScoped<IPayment, PaymentRepository>();
            services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
            services.AddScoped<IPaymentStatus, PaymentStatusRepository>();
            services.AddScoped<IVnpay, Vnpay>(
                provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var vnpayService = new Vnpay();
                    vnpayService.Initialize(
                        configuration["Vnpay:TmnCode"]!,
                        configuration["Vnpay:HashSecret"]!,
                        configuration["Vnpay:BaseUrl"]!,
                        configuration["Vnpay:CallbackUrl"]!
                    );
                    return vnpayService;
                }
            );
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateaPaymentConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("payment-service", e =>
                    {
                        e.ConfigureConsumer<CreateaPaymentConsumer>(context);
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
