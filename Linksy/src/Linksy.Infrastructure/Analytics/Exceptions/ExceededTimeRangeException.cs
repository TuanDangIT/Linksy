using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class ExceededTimeRangeException : LinksyException
    {
        public ExceededTimeRangeException(int limitInDays) : base($"The specified time range exceeds the maximum allowed limit in days: {limitInDays}.")
        {
        }
    }
}
