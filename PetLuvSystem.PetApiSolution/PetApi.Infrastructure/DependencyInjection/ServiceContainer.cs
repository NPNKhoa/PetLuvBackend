using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetApi.Application.Interfaces;
using PetApi.Infrastructure.Data;
using PetApi.Infrastructure.Repositories;
using PetApi.Infrastructure.Services;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;

namespace PetApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<PetDbContext>(services, config);

            CloudinaryHelper.ConfigureCloudinary(config);

            services.AddScoped<IPetType, PetTypeRepository>();
            services.AddScoped<IPetBreed, PetBreedRepository>();
            services.AddScoped<IPetHealthBook, PetHealthBookRepository>();
            services.AddScoped<IPetHealthBookDetail, PetHealthBookDetailRepository>();
            services.AddScoped<IPetType, PetTypeRepository>();
            services.AddScoped<IPet, PetRepository>();
            services.AddScoped<ISellingPet, SellingPetRepository>();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IPetCachingService, PetCachingService>();
            services.AddScoped<IBreedMappingCacheUpdateService, BreedMappingCacheUpdateService>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
