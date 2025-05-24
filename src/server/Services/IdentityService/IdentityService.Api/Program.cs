using CommonLibrary.Extensions;
using CommonLibrary.Middlewares;
using IdentityService.Api.Application.Services;
using IdentityService.Api.Application.Services.interfaces;
using IdentityService.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IIdentityService, IdentityService.Api.Application.Services.IdentityService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();

builder.Services.ConfigureConsul(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<IdentityDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["IdentityDbConnectionString"], sqlOptions =>
    {
        //sqlOptions.EnableRetryOnFailure(
        //    maxRetryCount: 5,
        //    maxRetryDelay: TimeSpan.FromSeconds(5),
        //    errorNumbersToAdd: null
        //);
    });
    opt.EnableSensitiveDataLogging();
});

//var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>()
//              .UseSqlServer(builder.Configuration["IdentityDbConnectionString"]);


//using var dbContext = new IdentityDbContext(optionsBuilder.Options);
//    dbContext.Database.Migrate();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    //if (app.Environment.IsProduction() && !await dbContext.Database.CanConnectAsync())
    //    dbContext.Database.Migrate();
}
app.UseCustomExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, builder.Configuration, conf =>
{
    conf.ID = "IdentityService-" + Guid.NewGuid();
    conf.Name = "IdentityService";
    conf.Tags = ["IdentityService", "Identity", "Jwt", "Token"];
});

app.Run();
