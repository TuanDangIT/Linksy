using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Exceptions
{
    public class LandingPageCodeInUseException(string code) : LinksyException($"Landing page with code: {code} is already in use.")
    {
    }
}
