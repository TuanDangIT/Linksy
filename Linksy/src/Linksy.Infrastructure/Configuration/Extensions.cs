using Linksy.Application.Shared.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Configuration
{
    internal static class Extensions
    {
        private static readonly string _linksyConfigSectionName = "LinksyConfiguration";
        public static IServiceCollection AddLinksyConfig(this IServiceCollection services)
        {
            var linksyConfig = services.GetOptions<LinksyConfig>(_linksyConfigSectionName);
            services.AddSingleton(linksyConfig);
            return services;
        }
    }
}
