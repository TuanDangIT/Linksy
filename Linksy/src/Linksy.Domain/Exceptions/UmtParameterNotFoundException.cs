using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class UmtParameterNotFoundException(int umtParameterId) : LinksyException($"Umt parameter: {umtParameterId} was not found.")
    {
    }
}
