using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    public class LinksyException(string message) : Exception(message)
    {
    }
}
