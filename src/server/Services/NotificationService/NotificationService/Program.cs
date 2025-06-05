using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;
using NotificationService.Models;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using NotificationService.Workers;
using RabbitMQ.Client;

namespace NotificationService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services, context.Configuration);

                })
                .Build();

            //var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            //host.Services.RegisterWithConsul(lifetime, configuration, conf =>
            //{
            //    conf.ID = "PaymentService-" + Guid.NewGuid();
            //    conf.Name = "PaymentService";
            //    conf.Tags = ["PaymentService", "Payment"];
            //});

            IEventBus eventBus = host.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

            Console.WriteLine("App is running...");
            host.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Worker>();
            services.AddLogging(conf => conf.AddConsole());
            services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
            services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
            services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpConfig"));
            services.AddSingleton<IEmailService, EmailService>();

            services.AddSingleton(sp =>
            {
                var config = new EventBusConfig
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "NotificationService",
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
        }
    }
}