using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;
using RabbitMQ.Client;

namespace PaymentService.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddLogging(config => config.AddConsole());
            services.AddTransient<OrderStartedIntegrationEventHandler>();
            services.AddSingleton<IEventBus>(sp =>
            {
                var config = new EventBusConfig
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "PaymentService",
                    EventBusType = EventBusType.RabbitMQ,
                    Connection = new ConnectionFactory
                    {
                        HostName = configuration.GetValue<string>("RabbitMQSettings:Host"),
                        Port = configuration.GetValue<int>("RabbitMQSettings:Port"),
                        UserName = configuration.GetValue<string>("RabbitMQSettings:Username"),
                        Password = configuration.GetValue<string>("RabbitMQSettings:Password")
                    }
                };
                return EventBusFactory.Create(config, sp);
            });
            services.ConfigureConsul(configuration);
            //services.ConfigureAuth(configuration);
            services.AddHealthChecks();

            return services;
        }

        public static WebApplication ConfigureRequestPipeline(this WebApplication app, IConfiguration configuration)
        {
            app.UseCustomExceptionHandling();
            app.MapHealthChecks("/health");

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.MapControllers();

            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            app.RegisterWithConsul(lifetime, configuration, conf =>
            {
                conf.ID = "PaymentService-" + Guid.NewGuid();
                conf.Name = "PaymentService";
                conf.Tags = ["PaymentService", "Payment"];
            });
            IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

            return app;
        }
    }
}
