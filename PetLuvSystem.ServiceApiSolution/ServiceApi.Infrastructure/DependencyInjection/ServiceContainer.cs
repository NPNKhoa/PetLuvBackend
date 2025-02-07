using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.DependencyInjections;
using Quartz;
using ServiceApi.Application.Interfaces;
using ServiceApi.Application.Jobs;
using ServiceApi.Infrastructure.Data;
using ServiceApi.Infrastructure.Repositories;
using ServiceApi.Infrastructure.Services;

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
            services.AddScoped<IServiceComboVariant, ServiceComboVariantRepository>();
            services.AddScoped<IServiceImage, ServiceImageRepository>();
            services.AddScoped<IServiceVariant, ServiceVariantRepository>();
            services.AddScoped<IWalkDogServiceVariant, WalkDogServiceVariantRepository>();

            services.AddHttpClient();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            services.AddScoped<IBreedMappingService, BreedMappingService>();

            services.AddQuartz(q =>
            {
                q.UseInMemoryStore();

                var jobKey = new JobKey("UpdateBreedMappingJob");
                q.AddJob<UpdateBreedMappingJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("UpdateBreedMappingTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithInterval(TimeSpan.FromHours(config.GetValue<int>("Quartz:JobIntervalInHours", 1)))
                        .RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
