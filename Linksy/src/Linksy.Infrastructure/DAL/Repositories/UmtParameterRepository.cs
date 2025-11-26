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
    internal class UmtParameterRepository : IUmtParameterRepository
    {
        private readonly LinksyDbContext _dbContext;

        public UmtParameterRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAsync(int umtParameterId, CancellationToken cancellationToken = default)
            => await _dbContext.UmtParameters
                .Where(up => up.Id == umtParameterId)
                .ExecuteDeleteAsync(cancellationToken);

        public Task<UmtParameter?> GetByIdAsync(int umtParameterId, CancellationToken cancellationToken = default, params Expression<Func<UmtParameter, object?>>[] includes)
        {
            var query = _dbContext.UmtParameters.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefaultAsync(u => u.Id == umtParameterId, cancellationToken);
        }

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
