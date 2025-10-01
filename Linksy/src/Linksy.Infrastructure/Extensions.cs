using Linksy.Infrastructure.Auth;
using Linksy.Infrastructure.BackgroundServices;
using Linksy.Infrastructure.Configuration;
using Linksy.Infrastructure.Contexts;
using Linksy.Infrastructure.DAL;
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
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgres(configuration);
            services.AddContext();
            services.AddLinksyConfig();
            services.AddAuth();
            services.AddVersioning();
            services.AddDocumentation();
            services.AddServices();
            services.AddBackgroundServices();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDocumentation();
            }
            app.UseHttpsRedirection();
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
