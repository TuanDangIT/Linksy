using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Exceptions
{
    internal class CustomCodeInUseException : LinksyException
    {
        public CustomCodeInUseException(string code) : base($"Custom code: {code} is already in use.")
        {
        }
    }
}
