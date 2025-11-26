using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class DescriptionFontRequiredException : LinksyException
    {
        public DescriptionFontRequiredException() : base("Description Font is required when Description is provided.")
        {
        }
    }
}
