using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Url;
using Linksy.Infrastructure.Pagination.Configuration;
using Linksy.Infrastructure.Pagination.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination
{
    internal static class Extension
    {
        public static IServiceCollection AddPagination(this IServiceCollection services)
        {
            services.AddSingleton<IPaginationConfiguration<QrCode>, QrCodePaginationConfiguration>();
            services.AddSingleton<IPaginationConfiguration<Barcode>, BarcodePaginationConfiguration>();
            services.AddSingleton<IPaginationConfiguration<Url>, UrlPaginationConfiguration>();
            services.AddSingleton<IPaginationConfiguration<LandingPage>, LandingPagePaginationConfiguration>();
            services.AddSingleton<IFilterService, FilterService>();
            services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
            return services;
        }
    }
}
