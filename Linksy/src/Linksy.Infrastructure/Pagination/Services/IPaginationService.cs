using Linksy.Application.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Services
{
    public interface IPaginationService<TEntity>
    {
        Task<PagedResult<TDto>> PaginateAsync</*TEntity,*/ TDto>(
            IQueryable<TEntity> query,
            int pageNumber,
            int pageSize,
            Dictionary<string, string>? filters,
            IEnumerable<string>? orders,
            Expression<Func<TEntity, TDto>> selector,
            //Dictionary<string, Expression<Func<TEntity, object>>>? allowedOrders = default,
            //HashSet<string>? allowedFilters = default,
            CancellationToken cancellationToken = default) /*where TEntity : class*/;
    }
}
