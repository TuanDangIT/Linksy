using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Exceptions
{
    internal class BackgroundConflictException : LinksyException
    {
        public BackgroundConflictException() : base("BackgroundColor and BackgroundImage cannot both be provided.")
        {
        }
    }
}
