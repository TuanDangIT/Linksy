using Linksy.Infrastructure.Analytics;
using Linksy.Infrastructure.Auth;
using Linksy.Infrastructure.BackgroundServices;
using Linksy.Infrastructure.BlobStorage;
using Linksy.Infrastructure.Configuration;
using Linksy.Infrastructure.Contexts;
using Linksy.Infrastructure.DAL;
using Linksy.Infrastructure.Pagination;
using Linksy.Infrastructure.ScanCodes;
using Linksy.Infrastructure.Services;
using Linksy.Infrastructure.Swagger;
using Linksy.Infrastructure.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPagination();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddPostgres(configuration);
            services.AddStatistics();
            services.AddContext();
            services.AddLinksyConfig();
            services.AddAuth();
            services.AddVersioning();
            services.AddDocumentation();
            services.AddBloblStorage(configuration);
            services.AddScanCodes();
            services.AddServices();
            services.AddBackgroundServices();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
               options.AddPolicy("AllowFrontEnd", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDocumentation();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontEnd");
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
        public static void RegisterOptions<T>(this IServiceCollection services, string sectionName) where T : class, new()
        {
            var options = services.GetOptions<T>(sectionName);
            services.AddSingleton(options);
        }
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
