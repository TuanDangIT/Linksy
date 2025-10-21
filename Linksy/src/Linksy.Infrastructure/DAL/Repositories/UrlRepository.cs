using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<Url?> GetUrlAsync(int id, CancellationToken cancellationToken = default)
            => _dbContext.Urls.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        public async Task CreateUrlAsync(Url url, CancellationToken cancellationToken = default)
        {
            await _dbContext.Urls.AddAsync(url, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
            => _dbContext.Urls.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);

        public async Task<bool> IsUrlCodeInUseAsync(string code, CancellationToken cancellationToken = default)
            => await _dbContext.Urls.AnyAsync(u => u.Code == code, cancellationToken);

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
