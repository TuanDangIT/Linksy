using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface IUmtParameterRepository
    {
        Task<UmtParameter?> GetByIdAsync(int umtParameterId, CancellationToken cancellationToken = default, params Expression<Func<UmtParameter, object?>>[] includes);
        Task DeleteAsync(int umtParameterId, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
