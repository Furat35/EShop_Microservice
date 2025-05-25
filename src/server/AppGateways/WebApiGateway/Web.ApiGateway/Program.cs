using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Configurations/ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services
    .AddOcelot()
    .AddConsul();
builder.Services.AddCors(conf =>
{
    conf.AddPolicy("frontEndConfig", builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("frontEndConfig");

app.UseAuthorization();

app.MapControllers();
await app.UseOcelot();

app.Run();
