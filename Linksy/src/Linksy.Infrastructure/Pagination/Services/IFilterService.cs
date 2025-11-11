using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Services
{
    public interface IFilterService
    {
        IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string filterValue);
    }
}
