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
    internal class BarcodeRepository : IBarcodeRepository
    {
        private readonly LinksyDbContext _dbContext;

        public BarcodeRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Barcode?> GetByIdAsync(int barcodeId, CancellationToken cancellationToken = default)
            => await _dbContext.Barcodes.FirstOrDefaultAsync(b => b.Id == barcodeId, cancellationToken);

        public async Task CreateAsync(Barcode barcode, CancellationToken cancellationToken = default)
        {
            await _dbContext.Barcodes.AddAsync(barcode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int barcodeId, bool includeUrlInDeletion, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Barcodes.AsQueryable();

            if (includeUrlInDeletion)
            {
                query = query.Include(b => b.Url);
            }

            var barcode = await query.FirstOrDefaultAsync(b => b.Id == barcodeId, cancellationToken);

            if (barcode is not null)
            {
                if (includeUrlInDeletion && barcode.Url is not null)
                {
                    _dbContext.Urls.Remove(barcode.Url);
                }

                _dbContext.Barcodes.Remove(barcode);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
