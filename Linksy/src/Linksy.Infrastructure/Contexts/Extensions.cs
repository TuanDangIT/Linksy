using Linksy.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Contexts
{
    internal static class Extensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IContextFactory, ContextFactory>();
            services.AddScoped(sp => sp.GetRequiredService<IContextFactory>().Create());
            return services;
        }
    }
}
