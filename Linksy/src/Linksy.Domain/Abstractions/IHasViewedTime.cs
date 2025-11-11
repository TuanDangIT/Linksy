using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Abstractions
{
    public interface IHasViewedTime
    {
        DateTime ViewedAt { get; }
    }
}
