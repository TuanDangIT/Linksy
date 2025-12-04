using Linksy.Application.Statistics.Analytics;
using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class InvalidTimeRangeException : LinksyException
    {
        private static readonly IEnumerable<string> _availableTimeRanges = Enum.GetNames<TimeRange>();
        public InvalidTimeRangeException(string givenTimeRange) : base($"Invalid time range: {givenTimeRange}. Allowed values: {string.Join(", ", _availableTimeRanges)}")
        {
        }
    }
}
