using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registrations.EventHandlerRegistration;
using OrderService.Api.Extensions.Registrations.ServiceDiscovery;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Configuration
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile($"appsettings.{env}.json", false)
    .AddEnvironmentVariables();
builder.Services
    .AddPersistenceRegistration(builder.Configuration)
    .AddApplicationRegistration(typeof(Program))
    .ConfigureEventHandlers()
    .AddServiceDiscoveryRegistration(builder.Configuration);

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
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        }
    };
    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDbContext<OrderDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderDbContext>>();

    var dbContextSeeder = new OrderDbContextSeed();
    dbContextSeeder.SeedAsync(context, logger)
        .Wait();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration);

app.Run();
