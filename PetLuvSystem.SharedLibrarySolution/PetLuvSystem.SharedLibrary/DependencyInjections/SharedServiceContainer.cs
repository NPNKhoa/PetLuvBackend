using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetLuvSystem.SharedLibrary.Middlewares;
using Serilog;

namespace PetLuvSystem.SharedLibrary.DependencyInjections
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(config.GetConnectionString("Default"),
                    sqlServerOption => sqlServerOption.EnableRetryOnFailure())
            );

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .CreateLogger();

            JWTAuthenticationScheme.AddJwtAuthenticationScheme(services, config);

            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();

            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
