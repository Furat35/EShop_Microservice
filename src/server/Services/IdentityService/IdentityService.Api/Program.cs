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
    opt.UseSqlServer(builder.Configuration["IdentityDbConnectionString"]);
    opt.EnableSensitiveDataLogging();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime, conf =>
{
    conf.ID = "IdentityService-" + Guid.NewGuid();
    conf.Name = "IdentityService";
    conf.Tags = ["IdentityService", "Identity", "Jwt", "Token"];
}, 5005);

app.Run();
