using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Configuration
{
    internal class LandingPagePaginationConfiguration : IPaginationConfiguration<LandingPage>
    {
        public Dictionary<string, Expression<Func<LandingPage, object>>> AllowedOrders => new(StringComparer.OrdinalIgnoreCase)
        {
            { "ViewCount", landingPage => landingPage.ViewCount },
            { "EngagementCount", landingPage => landingPage.EngagementCount },
            { "CreatedAt", landingPage => landingPage.CreatedAt },
            { "Id", landingPage => landingPage.Id },
        };

        public HashSet<string> AllowedFilters => new(StringComparer.OrdinalIgnoreCase)
        {
            "Code",
            "Title",
            "IsPublished",
            "Tags",
            "CreatedAt",
            "UpdatedAt",
            "ViewCount",
            "EngagementCount"
        };
    }
}
