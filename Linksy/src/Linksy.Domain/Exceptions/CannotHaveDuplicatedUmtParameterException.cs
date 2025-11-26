using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class CannotHaveDuplicatedUmtParameterException : LinksyException
    {
        public CannotHaveDuplicatedUmtParameterException()
            : base("Cannot have duplicated UMT parameter.")
        {
        }
    }
}
