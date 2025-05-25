using IdentityService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.ConfigureRequestPipeline(builder.Configuration);

app.Run();
