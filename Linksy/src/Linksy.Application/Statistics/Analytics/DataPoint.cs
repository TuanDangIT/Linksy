using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Analytics
{
    public record class DataPoint
    {
        public DateTime PeriodStart { get; init; }
        public int Count { get; init; }
        public int UniqueIpCount { get; init; }
        public DataPoint(DateTime periodStart, int count, int uniqueIpCount)
        {
            PeriodStart = periodStart;
            Count = count;
            UniqueIpCount = uniqueIpCount;
        }
    }
}
