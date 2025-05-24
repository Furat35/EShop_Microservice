using Discount.Api.Models;
using Discount.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using Discount.Api.Repositories;
using Discount.Api.Repositories.Context;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "DiscountApiService-" + Guid.NewGuid();
    conf.Name = "DiscountApiService";
    conf.Tags = ["DiscountApiService", "DiscountApi"];
});

app.MapGet("/discounts", ([FromServices] IDiscountRepository discountRepository) =>
{
    var discounts = discountRepository.GetAll().ToList();
    return discounts;
});

app.MapGet("/discounts/{id}", (int id, [FromServices] IDiscountRepository discountRepository) =>
{
    return discountRepository.GetByIdAsync(id);
});

app.MapPost("/discounts", async ([FromBody] Discount.Api.Models.Discount discount, [FromServices] IDiscountRepository discountRepository) =>
{
    await discountRepository.AddAsync(discount);
    return await discountRepository.SaveChangesAsync();
});

app.MapPost("/discounts/addToItem/{discountId:int}/{itemId:int}", async (int discountId, int itemId,
    [FromServices] IDiscountRepository discountRepository, [FromServices] ICatalogItemRepository productRepository) =>
{
    var discount = await discountRepository.GetByIdAsync(discountId, false);
    if (discount is null) throw new Exception("Discount doesn't exist");
    await productRepository.AddAsync(new CatalogItem { Id = itemId, DiscountId = discountId });
    return await productRepository.SaveChangesAsync();
});

app.MapPut("/discounts/addToItem/{discountId:int}/{itemId:int}", async (int discountId, int itemId,
    [FromServices] IDiscountRepository discountRepository, [FromServices] ICatalogItemRepository productRepository) =>
{
    var discount = await discountRepository.GetByIdAsync(discountId, false);
    if (discount is null) throw new Exception("Discount doesn't exist");
    productRepository.Update(new CatalogItem { Id = itemId, DiscountId = discountId });
    return await productRepository.SaveChangesAsync();
});

app.MapPut("/discounts", async ([FromBody] Discount.Api.Models.Discount discount, [FromServices] IDiscountRepository discountRepository) =>
{
    var discounts = discountRepository.Update(discount);
    await discountRepository.SaveChangesAsync();
    return discounts;
});

app.Run();
