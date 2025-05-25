using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using Discount.Api.Endpoints;
using Discount.Api.Repositories;
using Discount.Api.Repositories.Context;
using Discount.Api.Repositories.Interfaces;
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

builder.Services.ConfigureConsul(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
    dbContext.Database.Migrate();
}
app.UseCustomExceptionHandling();
app.MapHealthChecks("/health");

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "DiscountApiService-" + Guid.NewGuid();
    conf.Name = "DiscountApiService";
    conf.Tags = ["DiscountApiService", "DiscountApi"];
});

app.RegisterDiscountEndpoints();

app.Run();
