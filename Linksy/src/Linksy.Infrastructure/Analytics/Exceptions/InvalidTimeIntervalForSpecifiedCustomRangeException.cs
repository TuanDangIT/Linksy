using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class InvalidTimeIntervalForSpecifiedCustomRangeException : LinksyException
    {
        public InvalidTimeIntervalForSpecifiedCustomRangeException(string interval, string allowedIntervals) :
            base($"Invalid time interval for the custom range: {interval}. Allowed values: {allowedIntervals}")
        {

        }
    }
}
