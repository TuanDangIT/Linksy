using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Configuration
{
    public interface IPaginationConfiguration<T>
    {
        Dictionary<string, Expression<Func<T, object>>> AllowedOrders { get; }
        HashSet<string> AllowedFilters { get; }
    }
}
