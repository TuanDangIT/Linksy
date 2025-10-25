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
    internal class BarcodeRepository : IBarcodeRepository
    {
        private readonly LinksyDbContext _dbContext;

        public BarcodeRepository(LinksyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Barcode barcode, CancellationToken cancellationToken = default)
        {
            await _dbContext.Barcodes.AddAsync(barcode, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int barcodeId, bool includeUrlInDeletion, CancellationToken cancellationToken = default)
        {
            if(!includeUrlInDeletion)
            {
                await _dbContext.Barcodes.Where(b => b.Id == barcodeId).ExecuteDeleteAsync(cancellationToken);
                return;
            }
            var barcode = await _dbContext.Barcodes.Include(b => b.Url).FirstOrDefaultAsync(b => b.Id == barcodeId, cancellationToken);
            if(barcode is not null)
            {
                _dbContext.Urls.Remove(barcode.Url);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
