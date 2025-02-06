using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure.Data;
using UserApi.Infrastructure.Repository;

namespace UserApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<UserDbContext>(services, config);

            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IStaffDegree, StaffDegreeRepository>();
            services.AddScoped<IWorkSchedule, WorkScheduleRepository>();
            services.AddScoped<IWorkScheduleDetail, WorkScheduleDetailRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
