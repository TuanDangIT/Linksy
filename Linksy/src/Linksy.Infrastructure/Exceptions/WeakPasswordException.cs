using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class WeakPasswordException : LinksyException
    {
        public WeakPasswordException() : 
            base("Password must be at least 6 characters long and contain at least one digit, one lowercase letter, one uppercase letter, and one special character.")
        {
        }
    }
}
