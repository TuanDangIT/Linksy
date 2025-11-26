using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Linksy.Infrastructure.DAL.Repositories
{
    internal class LandingPageRepository : ILandingPageRepository
    {
        private readonly LinksyDbContext _dbContext;

        public LandingPageRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(LandingPage landingPage, CancellationToken cancellationToken = default)
        {
            await _dbContext.LandingPages.AddAsync(landingPage, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(int landingPageId, CancellationToken cancellationToken = default)
            => _dbContext.LandingPages
                .Where(lp => lp.Id == landingPageId)
                .ExecuteDeleteAsync(cancellationToken);

        public Task<LandingPage?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
            => _dbContext.LandingPages
                .FirstOrDefaultAsync(lp => lp.Code == code, cancellationToken);

        public async Task<LandingPage?> GetByIdAsync(int landingPageId, CancellationToken cancellationToken = default, params Expression<Func<LandingPage, object?>>[] includes)
        {
            var query = _dbContext.LandingPages.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var landingPage = await query.FirstOrDefaultAsync(u => u.Id == landingPageId, cancellationToken);
            return landingPage;
        }

        public async Task<LandingPage?> GetByIdWithoutQueryFilterAsync(int landingPageId, CancellationToken cancellationToken = default, params Expression<Func<LandingPage, object?>>[] includes)
        {
            var query = _dbContext.LandingPages
                .IgnoreQueryFilters()
                .AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var landingPage = await query.FirstOrDefaultAsync(u => u.Id == landingPageId, cancellationToken);
            return landingPage;
        }


        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
