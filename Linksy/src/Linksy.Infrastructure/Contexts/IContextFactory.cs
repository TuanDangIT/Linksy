using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Contexts
{
    internal interface IContextFactory
    {
        IContextService Create();
    }
}
