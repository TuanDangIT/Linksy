using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public async Task<QrCode?> GetByIdAsync(int qrCodeId, CancellationToken cancellationToken = default)
            => await _dbContext.QrCodes.FirstOrDefaultAsync(b => b.Id == qrCodeId, cancellationToken);
        public async Task CreateAsync(QrCode qrCode, CancellationToken cancellationToken = default)
        {
            await _dbContext.QrCodes.AddAsync(qrCode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int qrCodeId, bool includeUrlInDeletion, CancellationToken cancellationToken = default)
        {
            if (!includeUrlInDeletion)
            {
                await _dbContext.QrCodes.Where(b => b.Id == qrCodeId).ExecuteDeleteAsync(cancellationToken);
                return;
            }
            var qrCode = await _dbContext.QrCodes.Include(b => b.Url).FirstOrDefaultAsync(b => b.Id == qrCodeId, cancellationToken);
            if (qrCode is not null)
            {
                _dbContext.Urls.Remove(qrCode.Url);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }


        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
