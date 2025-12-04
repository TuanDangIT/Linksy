using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class InvalidStartDateException : LinksyException
    {
        public InvalidStartDateException(string message) : base(message)
        {
        }
    }
}
