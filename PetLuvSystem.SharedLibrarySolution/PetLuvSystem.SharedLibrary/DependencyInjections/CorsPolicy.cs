using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PetLuvSystem.SharedLibrary.DependencyInjections
{
    public static class CorsPolicy
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://localhost:5010", "http://localhost:5115")
                           .AllowAnyHeader()
                           .AllowAnyMethod();

                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors("AllowFrontend");
            return app;
        }
    }
}
