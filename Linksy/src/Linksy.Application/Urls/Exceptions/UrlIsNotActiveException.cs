using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Exceptions
{
    public class UrlIsNotActiveException(int urlId) : LinksyException($"URL with ID {urlId} is not active.")
    {
    }
}
