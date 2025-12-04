using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    internal class UmtParameterNotFoundException(int umtParamterId) : LinksyException($"UMT Parameter with the specified ID {umtParamterId} was not found.")
    {
    }
}
