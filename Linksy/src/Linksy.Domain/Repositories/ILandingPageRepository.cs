using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface ILandingPageRepository
    {
        Task<LandingPage?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<LandingPage?> GetByIdAsync(int landingPageId, CancellationToken cancellationToken = default, params Expression<Func<LandingPage, object?>>[] includes);
        Task<LandingPage?> GetByIdWithoutQueryFilterAsync(int landingPageId, CancellationToken cancellationToken = default, params Expression<Func<LandingPage, object?>>[] includes);
        Task CreateAsync(LandingPage landingPage, CancellationToken cancellationToken = default);
        Task DeleteAsync(int landingPageId, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task<bool> IsLandingPageCodeInUseAsync(string code, CancellationToken cancellationToken = default);
    }
}
