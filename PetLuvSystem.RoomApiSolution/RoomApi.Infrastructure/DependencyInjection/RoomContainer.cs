using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using RoomApi.Application.Interfaces;
using RoomApi.Infrastructure.Data;
using RoomApi.Infrastructure.Repositories;

namespace RoomApi.Infrastructure.DependencyInjection
{
    public static class RoomContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<RoomDbContext>(services, config);

            services.AddScoped<IAgreeableBreed, AgreeableBreedRepository>();
            services.AddScoped<IRoom, RoomRepository>();
            services.AddScoped<IRoomAccessory, RoomAccessoryRepository>();
            services.AddScoped<IRoomType, RoomTypeRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
