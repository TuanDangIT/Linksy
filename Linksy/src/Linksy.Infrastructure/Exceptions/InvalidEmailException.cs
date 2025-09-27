using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class InvalidEmailException : LinksyException
    {
        public InvalidEmailException() : base("Invalid email.")
        {
        }
    }
}
