using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using Discount.gRPC.Repositories;
using Discount.gRPC.Repositories.Context;
using Discount.gRPC.Repositories.Interfaces;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DiscountDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString"],
         sqlServerOptionsAction: sqlOptions =>
         {
             sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
             sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
         });
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
builder.Services.AddGrpc();
builder.Services.AddHealthChecks();
builder.Services.ConfigureConsul(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/health");
app.UseCustomExceptionHandling();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "DiscountGrpcService-" + Guid.NewGuid();
    conf.Name = "DiscountGrpcService";
    conf.Tags = ["DiscountGrpcService", "DiscountGrpc"];
});

app.MapGrpcService<DiscountService>();

app.Run();
