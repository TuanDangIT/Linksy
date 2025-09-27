using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Abstractions
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; protected set; } = default!;
    }
    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
