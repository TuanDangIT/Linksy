using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Analytics
{
    public record class AnalyticsSummary
    {
        public int Total { get; init; }
        public int TotalUniqueIps { get; init; }
        public double AverageCountPerInterval { get; init; }
        public int Peak { get; init; }
        public AnalyticsSummary(int total, int totalUniqueIps, double averageCountPerInterval, int peak)
        {
            Total = total;
            TotalUniqueIps = totalUniqueIps;
            AverageCountPerInterval = averageCountPerInterval;
            Peak = peak;
        }
    }
}
