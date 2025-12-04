using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetUmtParameterEngagementAnalytics
{
    public record class GetUmtParameterEngagementAnalytics(string TimeRange, string Interval, DateTime? StartDate, DateTime? EndDate) :
        AnalyticsRequest(TimeRange, Interval, StartDate, EndDate), IQuery<AnalyticsResponse>
    {
        [SwaggerIgnore]
        public int UmtParameterId { get; init; }    
    }
}
