using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Repositories
{
    internal class QrCodeRepository : IQrCodeRepository
    {
        private readonly LinksyDbContext _dbContext;

        public QrCodeRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(QrCode qrCode, CancellationToken cancellationToken = default)
        {
            await _dbContext.QrCodes.AddAsync(qrCode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
