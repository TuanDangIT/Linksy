using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class UserNotConfirmedException : LinksyException
    {
        public UserNotConfirmedException() : base("User has not been confirmed. Please confirm your account via mail.")
        {
        }
    }
}
