using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class CannotUpdateUmtParameterException : LinksyException
    {
        public CannotUpdateUmtParameterException(string message) : base(message)
        {
        }
    }
}
