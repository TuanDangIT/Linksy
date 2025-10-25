using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class CustomCodeInUseException(string code) : LinksyException($"Custom code: {code} is already in use.")
    {
    }
}
