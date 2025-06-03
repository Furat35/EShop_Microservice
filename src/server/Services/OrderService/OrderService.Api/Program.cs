using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registrations.EventHandlerRegistration;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Configuration
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile($"appsettings.{env}.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddApplicationRegistration(typeof(Program))
    .AddPersistenceRegistration(builder.Configuration)
    .ConfigureEventHandlers()
    .ConfigureConsul(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    var config = new EventBusConfig
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "OrderService",
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

var app = builder.Build();

app.UseCustomExceptionHandling();

app.MigrateDbContext<OrderDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderDbContext>>();

    var dbContextSeeder = new OrderDbContextSeed();
    dbContextSeeder.SeedAsync(context, logger)
        .Wait();
});


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapHealthChecks("/health");

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "OrderService-" + Guid.NewGuid();
    conf.Name = "OrderService";
    conf.Tags = new[] { "OrderService", "Order" };
});


IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.Run();
