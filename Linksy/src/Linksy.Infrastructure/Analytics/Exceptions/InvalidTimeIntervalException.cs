using Linksy.Application.Statistics.Analytics;
using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class InvalidTimeIntervalException : LinksyException
    {
        private static readonly IEnumerable<string> _availableTimeIntervals = Enum.GetNames<TimeInterval>();
        public InvalidTimeIntervalException(string givenTimeInterval) : 
            base($"Invalid time interval: {givenTimeInterval}. Allowed values: {string.Join(", ", _availableTimeIntervals)}")
        {
        }
    }
}
