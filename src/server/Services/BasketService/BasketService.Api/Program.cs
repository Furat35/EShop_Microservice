using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.Infrastructure.Services;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using CommonLibrary.Exceptions;
using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Grpc.Core;
using RabbitMQ.Client;
using services = BasketService.Api.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddGrpcClient<Discount.gRPC.DiscountService.DiscountServiceClient>(config =>
{
    config.Address = new Uri(builder.Configuration["GrpcSettings:Url"]!);
});
//.ConfigurePrimaryHttpMessageHandler(() =>
//{
//    return new HttpClientHandler
//    {
//        // Important for plaintext HTTP/2
//        SocketsHttpHandler = new SocketsHttpHandler
//        {
//            EnableMultipleHttp2Connections = true
//        }
//    };
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "BasketService-" + Guid.NewGuid();
    conf.Name = "BasketService";
    conf.Tags = ["BasketService", "Basket"];
});
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.Run();
