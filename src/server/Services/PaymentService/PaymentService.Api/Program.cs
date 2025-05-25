using PaymentService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPaymentServices(builder.Configuration);

var app = builder.Build();

app.ConfigureRequestPipeline(builder.Configuration);

app.Run();
