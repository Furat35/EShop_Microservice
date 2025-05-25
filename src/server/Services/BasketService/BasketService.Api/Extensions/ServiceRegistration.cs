using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.Infrastructure.Services;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using RabbitMQ.Client;
using services = BasketService.Api.Infrastructure.Services;


namespace BasketService.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCatalogServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddControllers();
            builder.Services.ConfigureAuth(builder.Configuration);
            builder.Services.ConfigureConsul(builder.Configuration);
            builder.Services.AddSingleton(sp => sp.ConfigureRedis(builder.Configuration));
            builder.Services.AddHealthChecks();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IEventBus>(sp =>
            {
                var config = new EventBusConfig
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "BasketService",
                    EventBusType = EventBusType.RabbitMQ,
                    Connection = new ConnectionFactory
                    {
                        HostName = builder.Configuration.GetValue<string>("RabbitMQSettings:Host"),
                        Port = builder.Configuration.GetValue<int>("RabbitMQSettings:Port"),
                        UserName = builder.Configuration.GetValue<string>("RabbitMQSettings:Username"),
                        Password = builder.Configuration.GetValue<string>("RabbitMQSettings:Password")
                    }
                };
                return EventBusFactory.Create(config, sp);
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IBasketService, services.BasketService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();
            builder.Services.AddGrpcClient<Discount.gRPC.DiscountService.DiscountServiceClient>(config => { config.Address = new Uri(builder.Configuration["GrpcSettings:Url"]!); });


            return builder.Services;
        }

        public static WebApplication ConfigureRequestPipeline(this WebApplication app, IConfiguration configuration)
        {
            app.UseCustomExceptionHandling();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            app.MapControllers();

            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            app.RegisterWithConsul(lifetime, configuration, conf =>
            {
                conf.ID = "BasketService-" + Guid.NewGuid();
                conf.Name = "BasketService";
                conf.Tags = ["BasketService", "Basket"];
            });
            IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

            return app;
        }
    }
}
