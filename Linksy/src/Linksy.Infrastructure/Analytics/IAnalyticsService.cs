using Linksy.Application.Statistics.Analytics;
using Linksy.Domain.Entities.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Statistics
{
    internal interface IAnalyticsService
    {
        Task<AnalyticsResponse> GetEngagementAnalyticsAsync<T>(IQueryable<T> query, AnalyticsRequest request, CancellationToken cancellationToken = default) where T : Engagement;
        Task<AnalyticsResponse> GetViewAnalyticsAsync<T>(IQueryable<T> query, AnalyticsRequest request, CancellationToken cancellationToken = default) where T : LandingPageView;
    }
}
