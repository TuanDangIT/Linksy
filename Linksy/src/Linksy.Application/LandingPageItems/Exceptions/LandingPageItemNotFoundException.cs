using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Exceptions
{
    internal class LandingPageItemNotFoundException(int landingPageId) : LinksyException($"Landing page item with ID: {landingPageId} was not found.")
    {
    }
}
