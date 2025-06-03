using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using IdentityService.Api.Application.Services;
using IdentityService.Api.Application.Services.interfaces;
using IdentityService.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace IdentityService.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddScoped<IIdentityService, Application.Services.IdentityService>();
            services.AddScoped<IAppUserService, AppUserService>();

            services.ConfigureConsul(configuration);
            services.AddHealthChecks();
            services.AddDbContext<IdentityDbContext>(opt =>
            {
                opt.UseSqlServer(configuration["IdentityDbConnectionString"], sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    );
                });
                opt.EnableSensitiveDataLogging();
            });

            return services;
        }

        public static WebApplication ConfigureRequestPipeline(this WebApplication app, IConfiguration configuration)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseCustomExceptionHandling();

            app.MapHealthChecks("/health");
            app.MapControllers();

            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            app.RegisterWithConsul(lifetime, configuration, conf =>
            {
                conf.ID = "IdentityService-" + Guid.NewGuid();
                conf.Name = "IdentityService";
                conf.Tags = ["IdentityService", "Identity", "Jwt", "Token"];
            });

            return app;
        }
    }
}
