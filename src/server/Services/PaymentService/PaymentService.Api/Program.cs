using CommonLibrary.Extensions;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;
using RabbitMQ.Client;
using CommonLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(config => config.AddConsole());
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();
builder.Services.AddSingleton<IEventBus>(sp =>
{
    var config = new EventBusConfig
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
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
builder.Services.ConfigureConsul(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandling();
app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "PaymentService-" + Guid.NewGuid();
    conf.Name = "PaymentService";
    conf.Tags = ["PaymentService", "Payment"];
});
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

app.Run();
