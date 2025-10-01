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
        public async Task CreateUrlAsync(Url url, CancellationToken cancellationToken = default)
        {
            await _dbContext.Urls.AddAsync(url, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsUrlCodeInUseAsync(string code, CancellationToken cancellationToken = default)
            => await _dbContext.Urls.AnyAsync(u => u.Code == code, cancellationToken);
    }
}
