using Linksy.Domain.Entities.Url;
using Linksy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Repositories
{
    internal class UrlRepository : IUrlRepository
    {
        private readonly LinksyDbContext _dbContext;

        public UrlRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Url?> GetUrlAsync(int urlId, CancellationToken cancellationToken = default, params Expression<Func<Url, object?>>[] includes)
        {
            var query = _dbContext.Urls.AsQueryable();
            foreach(var include in includes)
            {
                query = query.Include(include);
            }
            var url = await query.FirstOrDefaultAsync(u => u.Id == urlId, cancellationToken);
            return url;
        }
        public async Task CreateAsync(Url url, CancellationToken cancellationToken = default)
        {
            await _dbContext.Urls.AddAsync(url, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(int urlId, CancellationToken cancellationToken = default)
            => _dbContext.Urls.Where(u => u.Id == urlId).ExecuteDeleteAsync(cancellationToken);

        public async Task<bool> IsUrlCodeInUseAsync(string code, CancellationToken cancellationToken = default)
            => await _dbContext.Urls.AnyAsync(u => u.Code == code, cancellationToken);

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
