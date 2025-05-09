﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure.Data;
using UserApi.Infrastructure.Repository;
using UserApi.Infrastructure.Services;

namespace UserApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<UserDbContext>(services, config);

            CloudinaryHelper.ConfigureCloudinary(config);

            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IStaffDegree, StaffDegreeRepository>();
            services.AddScoped<IWorkSchedule, WorkScheduleRepository>();
            services.AddScoped<IWorkScheduleDetail, WorkScheduleDetailRepository>();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ICustomerCachingService, CustomerCachingService>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
