using Linksy.Application.Shared.ScanCodes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.ScanCodes
{
    internal static class Extensions
    {
        public static IServiceCollection AddScanCodes(this IServiceCollection services)
        {
            services.AddSingleton<IScanCodeService, ScanCodeService>();
            return services;
        }
    }
}
