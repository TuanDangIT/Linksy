using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Repositories
{
    internal class LandingPageItemRepository : ILandingPageItemRepository
    {
        private readonly LinksyDbContext _dbContext;

        public LandingPageItemRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync<T>(T landingPageItem, CancellationToken cancellationToken = default) where T : LandingPageItem
        {
            await _dbContext.LandingPageItems.AddAsync(landingPageItem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(int landingPageItemId, CancellationToken cancellationToken = default)
            => _dbContext.LandingPageItems
                .Where(lpi => lpi.Id == landingPageItemId)
                .ExecuteDeleteAsync(cancellationToken);

        public Task<LandingPageItem?> GetByIdAsync(int landingPageItemId, CancellationToken cancellationToken = default)
            => _dbContext.LandingPageItems
                .FirstOrDefaultAsync(lpi => lpi.Id == landingPageItemId, cancellationToken);

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
