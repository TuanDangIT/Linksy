using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Analytics
{
    public record class AnalyticsRequest
    {
        public string TimeRange { get; init; }
        public string Interval { get; init; } 
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public AnalyticsRequest(string timeRange, string interval, DateTime? startDate, DateTime? endDate)
        {
            TimeRange = timeRange;
            Interval = interval;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
