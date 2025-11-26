using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class TagsLengthExceededException : LinksyException
    {
        public TagsLengthExceededException() : base("Tags must contain fewer than 8 items.")
        {
        }
    }
}
