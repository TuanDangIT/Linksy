using Linksy.Infrastructure.Statistics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics
{
    internal static class Extensions
    {
        public static IServiceCollection AddStatistics(this IServiceCollection services)
        {
            services.AddSingleton<IAnalyticsService, AnalyticsService>();
            return services;
        }
    }
}
