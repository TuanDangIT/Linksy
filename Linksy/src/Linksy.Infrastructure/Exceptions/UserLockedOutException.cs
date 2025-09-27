using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class UserLockedOutException : LinksyException
    {
        public UserLockedOutException() : base("User was locked out. Please try again in few minutes.")
        {
        }
    }
}
