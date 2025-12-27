using Linksy.API.Exceptions;
using Linksy.API.Middlewares;
using Linksy.Application;
using Linksy.Infrastructure;
using Linksy.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<AutoTokenRefreshMiddleware>();
builder.Services.AddScoped<MultiTenancyMiddleware>();

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseMiddleware<AutoTokenRefreshMiddleware>();
app.UseInfrastructure();
app.UseMiddleware<MultiTenancyMiddleware>();

app.Run();
