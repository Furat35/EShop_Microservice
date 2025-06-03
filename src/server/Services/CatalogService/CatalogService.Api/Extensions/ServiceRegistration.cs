using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using CatalogService.Api.Infrastructure.Services;
using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;


namespace CatalogService.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCatalogServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.ConfigureDbContext(configuration);
            builder.Services.ConfigureAuth(builder.Configuration);
            builder.Services.AddControllers();
            builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
            builder.Services.Configure<CatalogSettings>(configuration.GetSection("CatalogSettings"));
            builder.Services.ConfigureConsul(configuration);
            builder.Services.AddScoped<ICatalogItemService, CatalogItemService>();
            builder.Services.AddScoped<ICatalogBrandService, CatalogBrandService>();
            builder.Services.AddScoped<ICatalogTypeService, CatalogTypeService>();
            builder.Services.AddHealthChecks();

            return builder.Services;
        }

        public static WebApplication ConfigureRequestPipeline(this WebApplication app, IConfiguration configuration)
        {
            app.UseCustomExceptionHandling();

            app.MigrateDbContext<CatalogContext>((context, services) =>
            {
                var env = app.Services.GetService<IWebHostEnvironment>();
                var logger = app.Services.GetService<ILogger<CatalogContextSeed>>();

                new CatalogContextSeed()
                    .SeedAsync(context, env, logger)
                    .Wait();
            });

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            app.MapControllers();

            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            app.RegisterWithConsul(lifetime, configuration, conf =>
            {
                conf.ID = "CatalogService-" + Guid.NewGuid();
                conf.Name = "CatalogService";
                conf.Tags = ["CatalogService", "Catalog"];

            });

            return app;
        }
    }
}
