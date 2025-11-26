using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Exceptions
{
    internal class LandingPageNotFoundException : LinksyException
    {
        public LandingPageNotFoundException(int landingPageId) : base($"Landing page with ID: {landingPageId} was not found.")
        {
        }
        public LandingPageNotFoundException(string code) : base($"Landing page with code: {code} was not found.")
        {
        }
    }
}
