using Linksy.Domain.Entities.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Domain.Repositories
{
    public interface ILandingPageItemRepository
    {
        Task<LandingPageItem?> GetByIdAsync(int landingPageItemId, CancellationToken cancellationToken = default);
        Task CreateAsync<T>(T landingPageItem, CancellationToken cancellationToken = default) where T : LandingPageItem;
        Task DeleteAsync(int landingPageItemId, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
