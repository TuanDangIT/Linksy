using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Analytics
{
    public record class AnalyticsResponse
    {
        public string TimeRange { get; init; } = string.Empty;
        public string TimeInterval { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public IEnumerable<DataPoint> DataPoints { get; init; } = [];
        public AnalyticsSummary Summary { get; init; } = default!;
        public AnalyticsResponse(string timeRange, string timeInterval, DateTime startDate, DateTime endDate, 
            IEnumerable<DataPoint> dataPoints, AnalyticsSummary summary)
        {
            TimeRange = timeRange;
            TimeInterval = timeInterval;
            StartDate = startDate;
            EndDate = endDate;
            DataPoints = dataPoints;
            Summary = summary;
        }
    }
}
