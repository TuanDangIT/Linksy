using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL
{
    internal static class Extensions 
    {
        private const string _postgresSectionName = "Postgres";
        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LinksyDbContext>(o => o.UseNpgsql(configuration.GetConnectionString(_postgresSectionName)));
            return services;
        }
    }
}
