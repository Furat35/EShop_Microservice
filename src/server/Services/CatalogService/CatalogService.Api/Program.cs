using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using CatalogService.Api.Infrastructure.Services;
using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
//builder.WebHost.UseWebRoot("Pics");
builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));
builder.Services.ConfigureConsul(builder.Configuration);
builder.Services.AddScoped<ICatalogItemService, CatalogItemService>();
builder.Services.AddScoped<ICatalogBrandService, CatalogBrandService>();
builder.Services.AddScoped<ICatalogTypeService, CatalogTypeService>();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandling();

// dockerda sorun yaþanmamasý için þimdilik bu þekilde kullanýlacak
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    if (app.Environment.IsProduction() && !await dbContext.Database.CanConnectAsync())
    {
        app.MigrateDbContext<CatalogContext>((context, services) =>
        {
            var env = services.GetService<IWebHostEnvironment>();
            var logger = services.GetService<ILogger<CatalogContextSeed>>();

            new CatalogContextSeed()
                .SeedAsync(context, env, logger)
                .Wait();
        });
    }
}
//app.MigrateDbContext<CatalogContext>((context, services) =>
//{
//    var env = services.GetService<IWebHostEnvironment>();
//    var logger = services.GetService<ILogger<CatalogContextSeed>>();

//    new CatalogContextSeed()
//        .SeedAsync(context, env, logger)
//        .Wait();
//});

app.UseStaticFiles();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "CatalogService-" + Guid.NewGuid();
    conf.Name = "CatalogService";
    conf.Tags = ["CatalogService", "Catalog"];

});

app.Run();
