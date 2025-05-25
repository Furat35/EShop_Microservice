using BasketService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCatalogServices(builder.Configuration);

var app = builder.Build();

app.ConfigureRequestPipeline(builder.Configuration);

app.Run();
