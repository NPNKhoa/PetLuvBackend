using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using ServiceApi.Application.Interfaces;
using ServiceApi.Infrastructure.Data;
using ServiceApi.Infrastructure.Repositories;

namespace ServiceApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<ServiceDbContext>(services, config);

            services.AddScoped<IServiceType, ServiceTypeRepository>();
            services.AddScoped<IService, ServiceRepository>();
            services.AddScoped<IServiceCombo, ServiceComboRepository>();
            services.AddScoped<IServiceComboMapping, ServiceComboMappingRepository>();
            services.AddScoped<IServiceComboPrice, ServiceComboPriceRepository>();
            services.AddScoped<IServiceImage, ServiceImageRepository>();
            services.AddScoped<IServicePrice, ServicePriceRepository>();
            services.AddScoped<IWalkDogServicePrice, WalkDogServicePriceRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
