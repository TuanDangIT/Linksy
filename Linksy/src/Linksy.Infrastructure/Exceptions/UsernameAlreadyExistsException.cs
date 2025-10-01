using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class UsernameAlreadyExistsException(string username) : LinksyException($"Username: {username} already exists.")
    {
    }
}
