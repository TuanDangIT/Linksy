using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Exceptions
{
    public class UrlNotFoundException : LinksyException
    {
        public UrlNotFoundException(string code) : base($"Url was not found for code: {code}.")
        {
        }
    }
}
