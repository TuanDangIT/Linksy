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
app.UseInfrastructure();
app.UseMiddleware<AutoTokenRefreshMiddleware>();
app.UseMiddleware<MultiTenancyMiddleware>();

app.Run();
