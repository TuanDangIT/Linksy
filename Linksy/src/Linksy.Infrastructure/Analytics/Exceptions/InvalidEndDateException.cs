using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Analytics.Exceptions
{
    internal class InvalidEndDateException : LinksyException
    {
        public InvalidEndDateException(string message) : base(message)
        {
        }
    }
}
